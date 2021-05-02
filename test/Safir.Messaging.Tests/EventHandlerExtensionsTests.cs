using Moq;
using Moq.AutoMock;
using Safir.Messaging.Tests.Fakes;
using Xunit;

namespace Safir.Messaging.Tests
{
    public class EventHandlerExtensionsTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IEventHandler> _eventHandler;

        public EventHandlerExtensionsTests()
        {
            _eventHandler = _mocker.GetMock<IEventHandler>();
        }

        [Fact]
        public void GetEventType_GetsGenericHandlerEventType()
        {
            var handler = new MockEventHandler();

            var type = handler.GetEventType();
            
            Assert.Equal(typeof(MockEvent), type);
        }

        [Fact]
        public void GroupByEvent_GroupsByEventType()
        {
            var handler = new MockEventHandler();
            var handlers = new[] { handler };

            var grouped = handlers.GroupByEvent();
            
            Assert.NotNull(grouped);
            var item = Assert.Single(grouped);
            Assert.Equal(typeof(MockEvent), item!.Key);
            var value = Assert.Single(item);
            Assert.Equal(handler, value);
        }

        [Fact]
        public void IsGenericHandler_ReturnsTrueWhenConcreteTypeIsGenericHandler()
        {
            var handler = new MockEventHandler();

            var result = handler.IsGenericHandler();
            
            Assert.True(result);
        }
    }
}
