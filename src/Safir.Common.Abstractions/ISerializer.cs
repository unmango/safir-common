using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.Common
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface ISerializer
    {
        T Deserialize<T>(ReadOnlySpan<byte> value);

        ValueTask<T> DeserializeAsync<T>(ReadOnlySpan<byte> value, CancellationToken cancellationToken = default);
        
        Span<byte> Serialize<T>(T value);

        ValueTask<Memory<byte>> SerializeAsync<T>(T value, CancellationToken cancellationToken = default);
    }
}
