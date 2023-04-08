using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Volo.Abp.AspNetCore.SignalR;

namespace Qna.Game.OnlineServer.SignalR.Main;

[Authorize]
// [SignalRHub] // TODO:enable this cause problem
public class MainHub : AbpHub
{

    public MainHub()
    {
    }

    // [SignalRHidden]
    public override async Task OnConnectedAsync()
    {
        Logger.LogInformation("onConnected");
        Logger.LogInformation("userId = " + CurrentUser.UserName);
        await base.OnConnectedAsync();
    }

    // [SignalRHidden]
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Logger.LogInformation("onDisconnected");
        return base.OnDisconnectedAsync(exception);
    }

    public async Task Hello()
    {
        Logger.LogInformation("hello from " + CurrentUser.UserName);
        await Clients.All.SendAsync("SomeOneHello", CurrentUser.UserName);
    }
}