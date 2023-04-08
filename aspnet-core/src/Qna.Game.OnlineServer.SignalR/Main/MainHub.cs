using Microsoft.AspNetCore.Authorization;
using Qna.Game.OnlineServer.SignalR.Helpers;
using Volo.Abp.AspNetCore.SignalR;

namespace Qna.Game.OnlineServer.SignalR.Main;

[Authorize]
public class MainHub : AbpHub<IMainHubClient>, IMainHubServer
{
    public override async Task OnConnectedAsync()
    {
        Logger.LogInformation("onConnected");
        Logger.LogInformation("userId = " + CurrentUser.UserName);
        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Logger.LogInformation("onDisconnected");
        return base.OnDisconnectedAsync(exception);
    }

    public async Task Hello()
    {
        Logger.LogInformation("hello from " + CurrentUser.UserName);
        await this.CurrentClientConnection().HelloAsync();
    }
}