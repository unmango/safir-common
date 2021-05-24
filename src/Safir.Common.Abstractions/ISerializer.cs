using JetBrains.Annotations;

namespace Safir.Common
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface ISerializer
    {
        byte[] Serialize<T>(T value);

        T Deserialize<T>(byte[] value);
    }
}
