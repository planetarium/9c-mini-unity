using System.Collections.Generic;

namespace Mini9C.Network
{
    public static class SubscribableExtensions
    {
        public static void AddTo(this ISubscribable subscriber, List<ISubscribable> subscribers)
        {
            subscribers.Add(subscriber);
        }
    }
}
