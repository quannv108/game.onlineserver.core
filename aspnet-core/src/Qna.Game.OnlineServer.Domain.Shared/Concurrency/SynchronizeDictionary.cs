using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Qna.Game.OnlineServer.Concurrency;

public class SynchronizeDictionary<TKey, TValue>
    where TValue: class
{
    private readonly Dictionary<TKey, TValue> _dictionary = new();
    private readonly Mutex _mutex = new();

    public TValue? GetOrDefault(TKey key)
    {
        TValue? v = null;
        if (_mutex.WaitOne())
        {
            v = _dictionary.GetOrDefault(key);
        }
        _mutex.ReleaseMutex();

        return v;
    }
    
    public void SetOrUpdate(TKey key, TValue value)
    {
        if (_mutex.WaitOne())
        {
            _dictionary[key] = value;
        }
        _mutex.ReleaseMutex();
    }

    public TValue? Remove(Func<TValue, bool> predicate)
    {
        TValue? removedItem = null;
        if (_mutex.WaitOne())
        {
            removedItem = _dictionary.RemoveAll(x => predicate(x.Value))
                .FirstOrDefault().Value;
        }
        _mutex.ReleaseMutex();

        return removedItem;
    }

    public int Count()
    {
        var count = 0;
        if(_mutex.WaitOne())
        {
            count = _dictionary.Count;
        }
        _mutex.ReleaseMutex();

        return count;
    }

    public List<TValue> ToList()
    {
        var l = new List<TValue>();
        if(_mutex.WaitOne())
        {
            l = _dictionary.Values.ToList();
        }
        _mutex.ReleaseMutex();

        return l;
    }

    public List<TValue> Where(Func<TValue, bool> predicate)
    {
        var l = new List<TValue>();
        if(_mutex.WaitOne())
        {
            l = _dictionary.Values.Where(x => predicate(x)).ToList();
        }
        _mutex.ReleaseMutex();

        return l;
    }

    public void TryRemove(TKey key, out TValue removedItem)
    {
        removedItem = null;
        if(_mutex.WaitOne())
        {
            removedItem = _dictionary.GetOrDefault(key);
            if(removedItem != null)
            {
                _dictionary.Remove(key);
            }
        }
        _mutex.ReleaseMutex();
    }
}