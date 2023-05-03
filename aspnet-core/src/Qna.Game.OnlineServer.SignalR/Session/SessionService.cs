using Microsoft.AspNetCore.SignalR;
using Qna.Game.OnlineServer.Session;
using Qna.Game.OnlineServer.Session.Events;
using Qna.Game.OnlineServer.Session.Managers;
using Qna.Game.OnlineServer.SignalR.Contracts.Hub.Core;
using Qna.Game.OnlineServer.SignalR.Hub.Core;
using Volo.Abp;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.EventBus;

namespace Qna.Game.OnlineServer.SignalR.Session;

[RemoteService(false)]
public class SessionService<THub, TClientAction> : SignalRServiceBase<THub, TClientAction>, ISessionService, 
    ILocalEventHandler<UserSessionRemovedEvent>
    where THub : AbpHub<TClientAction>
    where TClientAction : class, IHubClientActionBase
{
    private readonly IUserConnectionSessionManager _userConnectionSessionManager;

    public SessionService(
        IUserConnectionSessionManager userConnectionSessionManager)
    {
        _userConnectionSessionManager = userConnectionSessionManager;
    }

    public UserConnectionSession Find(Guid userId)
    {
        return _userConnectionSessionManager.GetByUserIdAsync(userId).Result;
    }

    public async Task AddAsync(Guid userId, string connectionId)
    {
        var session = await _userConnectionSessionManager.CreateOrUpdateAsync(userId, connectionId);
        Logger.LogInformation(
            $"added user {session.UserId} to session list: total users = " +
            $"{_userConnectionSessionManager.GetTotalSessionCountAsync().Result}");
    }

    public void Remove(Guid userId, string connectionId)
    {
        _userConnectionSessionManager.DeleteAsync(userId, connectionId);
    }

    public Task HandleEventAsync(UserSessionRemovedEvent eventData)
    {
        Logger.LogDebug($"SessionService: UserSessionRemovedEvent: {eventData.UserId}, " +
                        $"old connectionId = {eventData.ConnectionId}, reason = {eventData.Reason.ToString()}");
        if (eventData.Reason == ConnectionSessionDestroyReason.MultiSession)
        {
            Clients.Client(eventData.ConnectionId).MultiLoginDetectedAsync();
        }

        return Task.CompletedTask;
    }
}