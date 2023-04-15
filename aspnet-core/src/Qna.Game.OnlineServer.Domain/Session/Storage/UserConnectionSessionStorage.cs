using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Qna.Game.OnlineServer.Session.Storage;

public class UserConnectionSessionStorage : IUserConnectionSessionStorage
{
    /// <summary>
    /// key = userId
    /// </summary>
    /// 
    private readonly Dictionary<Guid, UserConnectionSession> _userConnectionSessions = new();

    private readonly Mutex _mutex = new Mutex();

    public UserConnectionSession GetByUserId(Guid userId)
    {
        UserConnectionSession session = null;
        if (_mutex.WaitOne())
        {
            session = _userConnectionSessions.GetOrDefault(userId);
        }
        _mutex.ReleaseMutex();

        return session;
    }

    public UserConnectionSession Delete(Guid userId, string connectionId)
    {
        UserConnectionSession removedItem = null;
        if (_mutex.WaitOne())
        {
            removedItem = _userConnectionSessions.RemoveAll(x => x.Key == userId
                                                                 && x.Value.ConnectionId == connectionId)
                .FirstOrDefault().Value;
        }
        _mutex.ReleaseMutex();

        return removedItem;
    }

    public void Update(UserConnectionSession session)
    {
        if (_mutex.WaitOne())
        {
            _userConnectionSessions[session.UserId] = session;
        }

        _mutex.ReleaseMutex();
    }

    public void Create(UserConnectionSession session)
    {
        if(_mutex.WaitOne())
        {
            _userConnectionSessions.TryAdd(session.UserId, session);
        }
        _mutex.ReleaseMutex();
    }

    public int Count()
    {
        int count = 0;
        if(_mutex.WaitOne())
        {
            count = _userConnectionSessions.Count;
        }
        _mutex.ReleaseMutex();

        return count;
    }
}