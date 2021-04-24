using System.Threading;
using System.Threading.Tasks;

namespace Safir.Messaging.MediatR
{
    public interface IEventHandler<in T>
        where T : IEvent
    {
        Task Handle(T @event, CancellationToken cancellationToken);
    }
}
