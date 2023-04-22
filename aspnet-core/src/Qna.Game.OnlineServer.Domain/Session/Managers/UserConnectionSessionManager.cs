using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Qna.Game.OnlineServer.GamePlay;
using Qna.Game.OnlineServer.GamePlay.Players;
using Qna.Game.OnlineServer.Session.Events;
using Qna.Game.OnlineServer.Session.Storage;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Local;

namespace Qna.Game.OnlineServer.Session.Managers;

public class UserConnectionSessionManager : DomainService, IUserConnectionSessionManager
{
    private readonly IUserConnectionSessionStorage _userConnectionSessionStorage;
    private readonly ILocalEventBus _localEventBus;

    public UserConnectionSessionManager(
        IUserConnectionSessionStorage userConnectionSessionStorage,
        ILocalEventBus localEventBus
        )
    {
        _userConnectionSessionStorage = userConnectionSessionStorage;
        _localEventBus = localEventBus;
    }
    
    public async Task<UserConnectionSession> CreateOrUpdateAsync(Guid userId, string connectionId)
    {
        var session = new UserConnectionSession(userId, connectionId);
        // TODO: get current selected player, for now just create dummy player
        session.CurrentPlayer = new GamePlayer(userId)
        {
            CurrentLevel = 1
        };

        var existing = _userConnectionSessionStorage.GetByUserId(userId);
        _userConnectionSessionStorage.CreateOrUpdate(session);
        if (existing != null)
        {
            Logger.LogInformation($"session of user {session.UserId} has been updated");

            await Task.Run(() => _localEventBus.PublishAsync(new UserSessionRemovedEvent
            {
                UserId = userId,
                ConnectionId = existing.ConnectionId,
                Reason = ConnectionSessionDestroyReason.MultiSession
            }, false));
        }
        else
        {
            Logger.LogInformation($"new session for user {session.UserId} has been created");
        }

        return session;
    }

    public Task<UserConnectionSession> GetByUserIdAsync(Guid userId)
    {
        var session = _userConnectionSessionStorage.GetByUserId(userId);
        if (session is null)
        {
            throw new UserFriendlyException(userId.ToString(), "NotFound");
        }
        return Task.FromResult(session);
    }

    public async Task DeleteAsync(Guid userId, string connectionId)
    {
        var session = _userConnectionSessionStorage.Delete(userId, connectionId);
        if (session != null)
        {
            await _localEventBus.PublishAsync(new UserSessionRemovedEvent
            {
                UserId = userId,
                ConnectionId = session.ConnectionId,
                Reason = ConnectionSessionDestroyReason.UnSpecific
            }, false);
        }
    }

    public Task<int> GetTotalSessionCountAsync()
    {
        return Task.FromResult(_userConnectionSessionStorage.Count());
    }
}