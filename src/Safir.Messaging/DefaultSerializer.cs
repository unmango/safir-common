using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MessagePack;
using MessagePack.Resolvers;
using Safir.Common;

namespace Safir.Messaging
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public sealed class DefaultSerializer : ISerializer
    {
        private static readonly MessagePackSerializerOptions _options = ContractlessStandardResolver.Options;
        private static readonly Lazy<ISerializer> _instance = new(() => new DefaultSerializer());

        public static ISerializer Instance => _instance.Value;

        public T Deserialize<T>(ReadOnlySpan<byte> value) => MessagePackSerializer.Deserialize<T>(value.ToArray(), _options);

        public ValueTask<T> DeserializeAsync<T>(ReadOnlySpan<byte> value, CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream(value.ToArray());
            return MessagePackSerializer.DeserializeAsync<T>(stream, _options, cancellationToken);
        }

        public Span<byte> Serialize<T>(T value) => MessagePackSerializer.Serialize(value, _options);

        public async ValueTask<Memory<byte>> SerializeAsync<T>(T value, CancellationToken cancellationToken = default)
        {
            await using var stream = new MemoryStream();
            await MessagePackSerializer.SerializeAsync(stream, value, _options, cancellationToken);
            return stream.ToArray();
        }
    }
}
