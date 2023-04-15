using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Qna.Game.OnlineServer.Concurrency;

public static class ConcurrentBagExtensions
{
    public static ConcurrentBag<T> Exclude<T>(this ConcurrentBag<T> bag, Predicate<T> predicate)
    {
        var remainingItems = bag.ToList();
        var count = remainingItems.RemoveAll(predicate);
        if (count != 0)
        {
            bag = new ConcurrentBag<T>(remainingItems);
        }

        return bag;
    }
}