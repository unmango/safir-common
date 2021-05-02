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
            var temp = new MockEventHandler();
            _mocker.Use(typeof(IEnumerable<IEventHandler>), new[] { temp });
            var manager = _mocker.CreateInstance<SubscriptionManager>();

            await manager.StartAsync(_cancellationToken);
            
            _eventBus.Verify(x => x.GetObservable<MockEvent>());
        }

        [Fact]
        public async Task StopAsync_StopsWithNoHandlers()
        {
            // Shouldn't do anything
            await _manager.StopAsync(_cancellationToken);
        }
    }
}
