namespace Safir.EventSourcing.FSharp

open System;
open System.Threading
open System.Threading.Tasks
open Safir.EventSourcing
open Safir.Messaging;

[<AbstractClass>]
type EventStore() =
    abstract AddAsync<'TAggregateId> : 'TAggregateId -> IEvent -> CancellationToken -> Task;
        
    abstract GetAsync<'TAggregateId> : Guid -> CancellationToken -> Task<IEvent>;

    interface IEventStore with
        member this.AddAsync<'TAggregateId>
            (
                aggregateId: 'TAggregateId,
                event: IEvent,
                cancellationToken: CancellationToken
            ) : Task =
            this.AddAsync aggregateId event cancellationToken

        member this.AddAsync<'TAggregateId>
            (
                aggregateId: 'TAggregateId,
                events: System.Collections.Generic.IEnumerable<IEvent>,
                cancellationToken: CancellationToken
            ) : Task =
            // TODO: Learn F# so I can inline this
            let addAsync x =
                this.AddAsync aggregateId x cancellationToken

            Task.WhenAll(Seq.map addAsync events)

        member this.GetAsync<'TAggregateId>
            (
                id: Guid,
                cancellationToken: CancellationToken
            ) : Task<IEvent> =
            this.GetAsync<'TAggregateId> id cancellationToken

        member this.GetAsync<'TEvent, 'TAggregateId when 'TEvent :> IEvent>
            (
                id: Guid,
                cancellationToken: CancellationToken
            ) : Task<'TEvent> =
            // this.GetAsync<'TAggregateId> id cancellationToken
            failwith "todo"
            
        member this.StreamAsync(aggregateId, startPosition, endPosition, cancellationToken) = failwith "todo"

        member this.StreamBackwardsAsync(aggregateId, count, cancellationToken) = failwith "todo"
