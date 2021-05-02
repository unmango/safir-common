using Moq.AutoMock;
using Xunit;

namespace Safir.Common.Tests
{
    public class TypeExtensionsTests
    {
        private readonly AutoMocker _mocker = new();

        [Fact]
        public void IsAssignableToGeneric_ReturnsTrueWhenConcreteIsClosedGenericOpenInterface()
        {
            var @interface = typeof(IOpen<>);
            var closed = typeof(Closed);

            var result = closed.IsAssignableToGeneric(@interface);
            
            Assert.True(result);
        }
        
        private interface IOpen<T> { }
        
        private class Closed : IOpen<int> { }
    }
}
