using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Safir.Messaging.MediatR
{
    internal sealed class EventHandlerAdapter<TEvent, TNotification> : INotificationHandler<TNotification>
        where TEvent : IEvent
        where TNotification : Notification<TEvent>
    {
        private readonly IEventHandler<TEvent> _eventHandler;

        public EventHandlerAdapter(IEventHandler<TEvent> eventHandler)
        {
            _eventHandler = eventHandler ?? throw new ArgumentNullException(nameof(eventHandler));
        }
        
        public Task Handle(TNotification notification, CancellationToken cancellationToken)
        {
            return _eventHandler.Handle(notification.Value, cancellationToken);
        }
    }
}
