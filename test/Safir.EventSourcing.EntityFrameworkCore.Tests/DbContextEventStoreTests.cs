using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.AutoMock;
using Safir.Messaging;
using Xunit;

namespace Safir.EventSourcing.EntityFrameworkCore.Tests
{
    public class DbContextEventStoreTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<TestContext> _context;
        private readonly Mock<DbSet<Event>> _eventSet;
        private readonly Mock<IEventSerializer> _serializer;
        private readonly DbContextEventStore<TestContext> _store;

        public DbContextEventStoreTests()
        {
            _context = _mocker.GetMock<TestContext>();
            _eventSet = _mocker.GetMock<DbSet<Event>>();
            _context.Setup(x => x.Set<Event>()).Returns(_eventSet.Object);
            _serializer = _mocker.GetMock<IEventSerializer>();
            _store = _mocker.CreateInstance<DbContextEventStore<TestContext>>();
        }

        [Fact]
        public async Task AddAsync_AddsAndSavesEvent()
        {
            const long id = 420;
            IEvent value = new MockEvent();
            var serialized = new Event(id, "type", ReadOnlyMemory<byte>.Empty, DateTime.Now, new Metadata(), 69);
            _serializer.Setup(x => x.SerializeAsync(id, value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(serialized)
                .Verifiable();

            await _store.AddAsync(id, value);
            
            _serializer.Verify();
            _context.Verify(x => x.AddAsync(serialized, It.IsAny<CancellationToken>()));
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async Task AddAsync_Enumerable_AddsAndSavesAllEvents(int count)
        {
            const long id = 420;
            var events = Enumerable.Repeat(new MockEvent(), count).Cast<IEvent>();
            var serialized = new Event(id, "type", ReadOnlyMemory<byte>.Empty, DateTime.Now, new Metadata(), 69);
            _serializer.Setup(x => x.SerializeAsync(id, It.IsAny<IEvent>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(serialized);

            await _store.AddAsync(id, events);
            
            _serializer.Verify(x => x.SerializeAsync(id, It.IsAny<IEvent>(), It.IsAny<CancellationToken>()), Times.Exactly(count));
            _context.Verify(x => x.AddRangeAsync(It.Is<IEnumerable<Event>>(y => y.Count() == count), It.IsAny<CancellationToken>()));
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GetAsync_GetsEventMatchingId()
        {
            
        }

        private record MockEvent : IEvent
        {
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public DateTime Occurred { get; }
        }
    }
}
