using Microsoft.AspNetCore.Authorization;
using Qna.Game.OnlineServer.SignalR.Contracts.Hub.Main;
using Qna.Game.OnlineServer.SignalR.Contracts.Match;
using Qna.Game.OnlineServer.SignalR.Hub.Core;
using Qna.Game.OnlineServer.SignalR.Match;

namespace Qna.Game.OnlineServer.SignalR.Hub.Main;

[Authorize]
public class MainHub : BaseHub<IMainClient>, IMainHubClientToServer
{
    private readonly IMatchService _matchService;

    public MainHub(
        IMatchService matchService)
    {
        _matchService = matchService;
    }
    
    public override async Task OnConnectedAsync()
    {
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

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = CurrentUser.Id!.Value;
        Logger.LogInformation(
            $"onDisconnected username =  {CurrentUser.UserName} in connectionId {Context.ConnectionId}");
        SessionService.Remove(userId, Context.ConnectionId);

        return base.OnDisconnectedAsync(exception);
    }

    #region client to server actions
    public async Task HelloAsync()
    {
        Logger.LogInformation("hello from " + CurrentUser.UserName);
        await CallerClient.HiAsync();
    }

    public async Task AutoJoinMatchAsync(AutoJoinMatchInput input)
    {
        try
        {
            var currentSession = CurrentUserConnectionSession;
            await _matchService.AutoJoinAsync(currentSession);
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex.Message);
        }
    }
    
    #endregion
}