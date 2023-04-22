using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Qna.Game.OnlineServer.Concurrency;

public class SynchronizeList<T> where T : class
{
    private readonly List<T> _items = new();
    private readonly Mutex _mutex = new();
    
    public int Count
    {
        get
        {
            int count = 0;
            if(_mutex.WaitOne())
            {
                count = _items.Count;
            }
            _mutex.ReleaseMutex();

            return count;
        }
    }
    
    public void Add(T item)
    {
        if(_mutex.WaitOne())
        {
            _items.Add(item);
        }
        _mutex.ReleaseMutex();
    }

    public T? First()
    {
        T? firstItem = null;
        if(_mutex.WaitOne())
        {
            firstItem = _items.FirstOrDefault();
        }
        _mutex.ReleaseMutex();
        return firstItem;
    }

    public bool Any(Func<T, bool> predicate)
    {
        var any = false;
        if(_mutex.WaitOne())
        {
            any = _items.Any(predicate);
        }
        _mutex.ReleaseMutex();
        return any;
    }

    public int RemoveAll(Predicate<T> predicate)
    {
        var removeCount = 0;
        if (_mutex.WaitOne())
        {
            removeCount = _items.RemoveAll(predicate);
        }
        _mutex.ReleaseMutex();

        return removeCount;
    }
    
    // public T Remove(Predicate<T> predicate)
    // {
    //     var removeCount = 0;
    //     if (_mutex.WaitOne())
    //     {
    //         removeCount = _items.RemoveAll(predicate);
    //     }
    //     _mutex.ReleaseMutex();
    //
    //     return removeCount;
    // }
    public List<T> ToList()
    {
        var l = new List<T>();
        if (_mutex.WaitOne())
        {
            l = _items.ToList();
        }
        _mutex.ReleaseMutex();
        return l;
    }
}