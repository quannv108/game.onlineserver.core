using Microsoft.AspNetCore.SignalR;
using Qna.Game.OnlineServer.Test;
using SignalRSwaggerGen.Attributes;
using Volo.Abp.AspNetCore.SignalR;

namespace Qna.Game.OnlineServer.SignalR.Test;

[SignalRHub]
public class TestHub : AbpHub
{
    public override Task OnConnectedAsync()
    {
        Logger.LogInformation("onConnected");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Logger.LogInformation("onDisconnected");
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendAll(TestMessageDto message)
    {
        await Clients.All.SendAsync("HandleMessage", message);
    }
}