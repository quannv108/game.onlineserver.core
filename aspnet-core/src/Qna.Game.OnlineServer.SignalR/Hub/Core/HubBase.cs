using Qna.Game.OnlineServer.Maintenance.Managers;
using Qna.Game.OnlineServer.Session;
using Qna.Game.OnlineServer.SignalR.Contracts.Hub.Core;
using Qna.Game.OnlineServer.SignalR.Contracts.Match;
using Qna.Game.OnlineServer.SignalR.Contracts.Users;
using Qna.Game.OnlineServer.SignalR.Match;
using Qna.Game.OnlineServer.SignalR.Session;
using SignalRSwaggerGen.Attributes;
using Volo.Abp.AspNetCore.SignalR;

namespace Qna.Game.OnlineServer.SignalR.Hub.Core;

[SignalRHub]
public abstract class HubBase<THub, TClientAction> : AbpHub<TClientAction>
    where THub: AbpHub<TClientAction>
    where TClientAction : class, IHubClientActionBase
{
    protected UserConnectionSession CurrentUserConnectionSession => SessionService.Find(CurrentUser.Id!.Value);
    protected TClientAction CallerClient => Clients.Client(Context.ConnectionId);

    protected ISessionService SessionService =>
        LazyServiceProvider.LazyGetRequiredService<SessionService<THub, TClientAction>>();

    protected IMatchService MatchService =>
        LazyServiceProvider.LazyGetRequiredService<MatchService<THub, TClientAction>>();

    private IMaintenanceScheduleManager MaintenanceScheduleManager =>
        LazyServiceProvider.LazyGetRequiredService<IMaintenanceScheduleManager>();

    [SignalRHidden]
    public override async Task OnConnectedAsync()
    {
        if (await MaintenanceScheduleManager.IsSignalROnMaintenanceAsync())
        {
            Context.Abort();
        }
        
        var connectionId = Context.ConnectionId;
        Logger.LogInformation($"onConnected username = {CurrentUser.UserName} in connectionId {connectionId}");
        var userId = CurrentUser.Id!.Value;
        try
        {
            await SessionService.AddAsync(userId, Context.ConnectionId);
        }
        catch (OverflowException ex)
        {
            Logger.LogWarning(ex.Message, ex);
            Context.Abort();
        }

        await base.OnConnectedAsync();
    }

    [SignalRHidden]
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = CurrentUser.Id!.Value;
        Logger.LogInformation(
            $"onDisconnected username =  {CurrentUser.UserName} in connectionId {Context.ConnectionId}");
        SessionService.Remove(userId, Context.ConnectionId);

        return base.OnDisconnectedAsync(exception);
    }

    #region ServerActions
    public async Task HelloAsync()
    {
        Logger.LogInformation("hello from " + CurrentUser.UserName);
        var session = CurrentUserConnectionSession;
        await CallerClient.HiAsync(new GamePlayerDto
        {
            Id = session.CurrentPlayer.Id,
            UserId = session.UserId,
            CurrentLevel = session.CurrentPlayer.CurrentLevel
        });
    }

    public async Task AutoJoinMatchAsync(AutoJoinMatchInput input)
    {
        try
        {
            var currentSession = CurrentUserConnectionSession;
            await MatchService.AutoJoinAsync(currentSession, input.GameId);
        }
        catch (Exception e)
        {
            Logger.LogWarning(e.Message, e);
        }
    }

    public async Task LeaveMatchAsync()
    {
        try
        {
            await MatchService.LeaveMatchAsync(CurrentUserConnectionSession);
        }
        catch (Exception e)
        {
            Logger.LogWarning(e.Message, e);
        }
    }
    
    #endregion
}