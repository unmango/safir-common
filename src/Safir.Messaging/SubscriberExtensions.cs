using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MessagePack;
using MessagePack.Resolvers;
using StackExchange.Redis;

namespace Safir.Messaging
{
    // TODO: Handle abstractions in generic args
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class SubscriberExtensions
    {
        private static MessagePackSerializerOptions _serializerOptions = ContractlessStandardResolver.Options;
        
        public static IObservable<T> CreateObservable<T>(this ISubscriber subscriber, RedisChannel channel)
        {
            return Observable.Create<T>(observer => subscriber.SubscribeAsync(channel, (_, value) => {
                var message = MessagePackSerializer.Deserialize<T>(value, _serializerOptions);
                observer.OnNext(message);
            }));
        }
        
        public static IObservable<T> AsObservable<T>(this ChannelMessageQueue queue)
        {
            return Observable.Create<T>(observer => () => {
                queue.OnMessage(channelMessage => {
                    var message = MessagePackSerializer.Deserialize<T>(channelMessage.Message, _serializerOptions);
                    observer.OnNext(message);
                });
            });
        }

        public static Task<long> PublishAsync<T>(this ISubscriber subscriber, RedisChannel channel, T message)
        {
            var serialized = MessagePackSerializer.Serialize(message, _serializerOptions);
            return subscriber.PublishAsync(channel, serialized);
        }
    }
}
