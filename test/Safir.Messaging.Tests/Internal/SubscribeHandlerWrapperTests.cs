using System;
using Moq;
using Moq.AutoMock;
using Safir.Messaging.Internal;
using Safir.Messaging.Tests.Fakes;
using Xunit;

namespace Safir.Messaging.Tests.Internal
{
    public class SubscribeHandlerWrapperTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IEventBus> _eventBus;
        private readonly Mock<IEventHandler> _genericHandler;
        private readonly Mock<IEventHandler<MockEvent>> _typedHandler;
        private readonly SubscribeHandlerWrapper<MockEvent> _wrapper = new();

        public SubscribeHandlerWrapperTests()
        {
            _eventBus = _mocker.GetMock<IEventBus>();
            _genericHandler = _mocker.GetMock<IEventHandler>();
            _typedHandler = _mocker.GetMock<IEventHandler<MockEvent>>();
        }

        [Fact]
        public void Subscribe_ThrowsWhenInvalidHandlerType()
        {
            Assert.Throws<InvalidOperationException>(
                () => _wrapper.Subscribe(_eventBus.Object, _genericHandler.Object));
        }
    }
}
