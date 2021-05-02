using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Safir.Common;

namespace Safir.Messaging
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class EventHandlerExtensions
    {
        public static Type GetEventType(this IEventHandler handler)
        {
            if (!IsGenericHandler(handler))
            {
                throw new InvalidOperationException("Handler is not generic handler");
            }

            var type = handler.GetType()
                .GetInterfaces()
                .FirstOrDefault(IsGenericHandler);

            if (type == null) throw new InvalidOperationException("Unable to get event type from non-generic handler");
            
            return type.GetGenericArguments()[0];
        }
        
        public static IEnumerable<IGrouping<Type, IEventHandler>> GroupByEvent(this IEnumerable<IEventHandler> handlers)
        {
            return handlers.Where(IsGenericHandler).GroupBy(GetEventType);
        }

        public static bool IsGenericHandler(this Type type)
        {
            return type.IsAssignableToGeneric(typeof(IEventHandler<>));
        }

        public static bool IsGenericHandler(this IEventHandler handler)
        {
            return handler.GetType().IsGenericHandler();
        }

        private static bool TryGetGenericHandlerType(this IEventHandler handler, out Type? type)
        {
            type = null;
            
            if (!handler.IsGenericHandler()) return false;

            type = handler.GetEventType();

            return true;
        }
    }
}
