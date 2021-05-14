using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Safir.Messaging.Tests.Fakes;
using Xunit;

namespace Safir.Messaging.Tests
{
    public class SubscriptionManagerTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IEventBus> _eventBus;
        private readonly SubscriptionManager _manager;
        private readonly CancellationToken _cancellationToken = default;

        public SubscriptionManagerTests()
        {
            _eventBus = _mocker.GetMock<IEventBus>();
            _manager = _mocker.CreateInstance<SubscriptionManager>();
        }

        [Fact]
        public async Task StartAsync_StartsWithNoHandlers()
        {
            await _manager.StartAsync(_cancellationToken);
            
            _eventBus.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task StartAsync_StartsWithSingleHandler()
        {
            var handler = _mocker.GetMock<IEventHandler<MockEvent>>();
            _mocker.Use(typeof(IEnumerable<IEventHandler>), new[] { handler.Object });
            var manager = _mocker.CreateInstance<SubscriptionManager>();
            var expectedEvent = new MockEvent();
            Action<MockEvent>? capturedCallback = null;
            _eventBus.Setup(x => x.SubscribeAsync(It.IsAny<Action<MockEvent>>(), It.IsAny<CancellationToken>()))
                .Callback<Action<MockEvent>, CancellationToken>((x, _) => capturedCallback = x);

            await manager.StartAsync(_cancellationToken);
            
            _eventBus.Verify(x => x.SubscribeAsync(It.IsAny<Action<MockEvent>>(), _cancellationToken));
            
            capturedCallback?.Invoke(expectedEvent);
            
            handler.Verify(x => x.HandleAsync(expectedEvent, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task StopAsync_StopsWithNoHandlers()
        {
            await _manager.StartAsync(_cancellationToken);
            // Shouldn't do anything
            await _manager.StopAsync(_cancellationToken);
        }

        [Fact]
        public async Task StopAsync_StopsWithSingleHandler()
        {
            var handler = _mocker.GetMock<IEventHandler<MockEvent>>();
            _mocker.Use(typeof(IEnumerable<IEventHandler>), new[] { handler.Object });
            var manager = _mocker.CreateInstance<SubscriptionManager>();
            var disposable = _mocker.GetMock<IDisposable>();
            _eventBus.Setup(x => x.SubscribeAsync(It.IsAny<Action<MockEvent>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(disposable.Object);

            await manager.StartAsync(_cancellationToken);
            await manager.StopAsync(_cancellationToken);
            
            disposable.Verify(x => x.Dispose());
        }
    }
}
