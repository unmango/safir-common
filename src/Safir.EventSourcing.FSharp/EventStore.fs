namespace Safir.EventSourcing.FSharp

open Safir.EventSourcing

type EventStore() =
    interface IEventStore with
        member this.AddAsync<'TAggregateId>
            (
                aggregateId: 'TAggregateId,
                event: Safir.Messaging.IEvent,
                cancellationToken: System.Threading.CancellationToken
            ) : System.Threading.Tasks.Task =
            failwith "todo"

        member this.AddAsync<'TAggregateId>
            (
                aggregateId: 'TAggregateId,
                events: System.Collections.Generic.IEnumerable<Safir.Messaging.IEvent>,
                cancellationToken: System.Threading.CancellationToken
            ) : System.Threading.Tasks.Task =
            failwith "todo"

        member this.GetAsync<'TAggregateId>
            (
                id: System.Guid,
                cancellationToken: System.Threading.CancellationToken
            ) : System.Threading.Tasks.Task<Safir.Messaging.IEvent> =
            failwith "todo"

        member this.GetAsync<'TEvent, 'TAggregateId when 'TEvent :> Safir.Messaging.IEvent>
            (
                id: System.Guid,
                cancellationToken: System.Threading.CancellationToken
            ) : System.Threading.Tasks.Task<'TEvent> =
            failwith "todo"

        member this.StreamAsync(aggregateId, startPosition, endPosition, cancellationToken) = failwith "todo"
        
        member this.StreamBackwardsAsync(aggregateId, count, cancellationToken) = failwith "todo"
