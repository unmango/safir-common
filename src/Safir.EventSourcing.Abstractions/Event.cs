using System;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public sealed record Metadata(Guid CorrelationId, Guid CausationId);

    [PublicAPI]
    public record Event(
        string Type,
        long AggregateId,
        ReadOnlyMemory<byte> Data,
        DateTime Occurred,
        Metadata Metadata,
        int Version)
    {
        public long Id { get; init; }
        
        public int Position { get; init; }
    }
}
