using System;
using System.Reactive.Subjects;
using System.Threading;
using Moq;
using Moq.AutoMock;
using Safir.Messaging.Tests.Fakes;
using Xunit;

// ReSharper disable ConvertToLocalFunction

namespace Safir.Messaging.Tests
{
    public class EventBusExtensionsTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IEventBus> _eventBus;

        public EventBusExtensionsTests()
        {
            _eventBus = _mocker.GetMock<IEventBus>();
        }

        [Fact]
        public void Subscribe_SubscribesActionCallback()
        {
            var observable = new Subject<MockEvent>();
            _eventBus.Setup(x => x.GetObservable<MockEvent>()).Returns(observable);
            var flag = false;
            Action<MockEvent> callback = _ => flag = true;

            _eventBus.Object.Subscribe(callback);
            observable.OnNext(new());

            _eventBus.Verify(x => x.GetObservable<MockEvent>());
            Assert.True(flag);
        }

        [Fact]
        public void Subscribe_SubscribesEventHandler()
        {
            var observable = new Subject<MockEvent>();
            _eventBus.Setup(x => x.GetObservable<MockEvent>()).Returns(observable);
            var handler = _mocker.GetMock<IEventHandler<MockEvent>>();

            var subscription = _eventBus.Object.Subscribe(handler.Object);

            Assert.NotNull(subscription);
            _eventBus.Verify(x => x.GetObservable<MockEvent>());
            
            observable.OnNext(new());
            
            handler.Verify(x => x.HandleAsync(It.IsAny<MockEvent>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public void Subscribe_ThrowsWhenTypeIsNotEventType()
        {
            var handler = _mocker.Get<IEventHandler<MockEvent>>();

            Assert.Throws<InvalidOperationException>(
                () => _eventBus.Object.Subscribe(typeof(object), new[] { handler }));
        }

        [Fact]
        public void Subscribe_SubscribesNonGenericHandlerToEventType()
        {
            var observable = new Subject<MockEvent>();
            _eventBus.Setup(x => x.GetObservable<MockEvent>()).Returns(observable);
            var handler = _mocker.GetMock<IEventHandler<MockEvent>>();

            var subscriptions = _eventBus.Object.Subscribe(typeof(MockEvent), new[] { handler.Object });
            
            Assert.NotNull(subscriptions);
            Assert.Single(subscriptions);
            _eventBus.Verify(x => x.GetObservable<MockEvent>());
            
            observable.OnNext(new());
            
            handler.Verify(x => x.HandleAsync(It.IsAny<MockEvent>(), It.IsAny<CancellationToken>()));
        }
    }
}
