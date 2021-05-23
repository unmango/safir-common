using System;

namespace Safir.EventSourcing
{
    public sealed record Metadata(Guid CorrelationId, Guid CausationId);
    
    public record Event(
        long Id,
        string Type,
        long AggregateId,
        ulong Position,
        DateTime Occurred,
        Span<byte> Data,
        Metadata Metadata,
        int Version);
}
