using Microsoft.Extensions.Options;
using Moq.AutoMock;
using Safir.Messaging.Configuration;
using Safir.Redis.Configuration;
using Xunit;

namespace Safir.Messaging.Tests.Configuration
{
    public class ConfigureRedisOptionsTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly MessagingOptions _messagingOptions = new();

        public ConfigureRedisOptionsTests()
        {
            var mockOptions = _mocker.GetMock<IOptions<MessagingOptions>>();
            mockOptions.SetupGet(x => x.Value).Returns(_messagingOptions);
        }

        [Fact]
        public void SetsRedisConfiguration()
        {
            const string connectionString = "connection string";
            _messagingOptions.ConnectionString = connectionString;
            var configureOptions = _mocker.CreateInstance<ConfigureRedisOptions>();
            var redisOptions = new RedisOptions();

            configureOptions.Configure(redisOptions);
            
            Assert.Equal(connectionString, redisOptions.Configuration);
        }
    }
}
