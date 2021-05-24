using System;
using JetBrains.Annotations;

namespace Safir.Common
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface ISerializer
    {
        Span<byte> Serialize<T>(T value);

        T Deserialize<T>(Span<byte> value);
    }
}
