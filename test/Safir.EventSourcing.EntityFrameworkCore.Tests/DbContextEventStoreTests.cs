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
    public class DbContextEventStoreTests : IAsyncDisposable
    {
        private readonly AutoMocker _mocker = new();
        private readonly TestContext _context = new();
        private readonly Mock<IEventSerializer> _serializer;
        private readonly DbContextEventStore<TestContext> _store;

        public DbContextEventStoreTests()
        {
            _mocker.Use(_context);
            _serializer = _mocker.GetMock<IEventSerializer>();
            _store = _mocker.CreateInstance<DbContextEventStore<TestContext>>();
        }

        [Fact]
        public async Task AddAsync_AddsAndSavesEvent()
        {
            const long id = 420;
            IEvent value = new MockEvent();
            var serialized = new Event(id, "type", Array.Empty<byte>(), DateTime.Now, new Metadata(), 69);
            _serializer.Setup(x => x.SerializeAsync(id, value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(serialized)
                .Verifiable();

            await _store.AddAsync(id, value);
            
            _serializer.Verify();
            Assert.Contains(serialized, _context.Set<Event>());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async Task AddAsync_Enumerable_AddsAndSavesAllEvents(int count)
        {
            const long id = 420;
            var events = Enumerable.Repeat(new MockEvent(), count).Cast<IEvent>();
            var serialized = new Event(id, "type", Array.Empty<byte>(), DateTime.Now, new Metadata(), 69);
            _serializer.Setup(x => x.SerializeAsync(id, It.IsAny<IEvent>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(serialized);

            await _store.AddAsync(id, events);
            
            _serializer.Verify(x => x.SerializeAsync(id, It.IsAny<IEvent>(), It.IsAny<CancellationToken>()), Times.Exactly(count));
            Assert.Contains(serialized, _context.Set<Event>());
        }

        [Fact]
        public async Task GetAsync_GetsEventMatchingId()
        {
            var serialized = new Event(420, "type", Array.Empty<byte>(), DateTime.Now, new Metadata(), 69);
            var entity = await _context.AddAsync(serialized);
            await _context.SaveChangesAsync();

            await _store.GetAsync(entity.Entity.Id);
            
            _serializer.Verify(x => x.DeserializeAsync(serialized, It.IsAny<CancellationToken>()));
        }

        private record MockEvent : IEvent
        {
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public DateTime Occurred { get; }
        }

        public ValueTask DisposeAsync()
        {
            return _context.DisposeAsync();
        }
    }
}
