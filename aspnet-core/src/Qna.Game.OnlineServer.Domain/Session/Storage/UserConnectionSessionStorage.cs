using System;
using Qna.Game.OnlineServer.Concurrency;

namespace Qna.Game.OnlineServer.Session.Storage;

public class UserConnectionSessionStorage : IUserConnectionSessionStorage
{
    /// <summary>
    /// key = userId
    /// </summary>
    /// 
    private readonly SynchronizeDictionary<Guid, UserConnectionSession> _userConnectionSessions = new();

    public UserConnectionSession GetByUserId(Guid userId)
    {
        return _userConnectionSessions.GetOrDefault(userId);
    }

    public UserConnectionSession Delete(Guid userId, string connectionId)
    {
        return _userConnectionSessions.Remove(x => x.UserId == userId
                                            && x.ConnectionId == connectionId);
    }

    public int Count()
    {
        return _userConnectionSessions.Count();
    }

    public void CreateOrUpdate(UserConnectionSession session)
    {
        _userConnectionSessions.SetOrUpdate(session.UserId, session);
    }
}