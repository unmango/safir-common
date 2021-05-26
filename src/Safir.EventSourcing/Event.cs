using System;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
    public sealed record Metadata(Guid CorrelationId, Guid CausationId);

    public record Event(
        string Type,
        long AggregateId,
        ReadOnlyMemory<byte> Data,
        DateTime Occurred,
        Metadata Metadata,
        int Version)
    {
        public long Id { get; [UsedImplicitly] init; }
        
        public ulong Position { get; [UsedImplicitly] init; }
    }
}
