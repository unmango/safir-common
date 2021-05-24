using System;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
    public sealed record Metadata(Guid CorrelationId, Guid CausationId);

    public record Event(
        string Type,
        long AggregateId,
        ulong Position,
        DateTime Occurred,
        Span<byte> Data,
        Metadata Metadata,
        int Version)
    {
        public long Id { get; [UsedImplicitly] init; }
    }
}
