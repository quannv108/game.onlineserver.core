using Volo.Abp.AspNetCore.SignalR;

namespace Qna.Game.OnlineServer.SignalR.Helpers;

public static class AbpHubExtensions
{
    public static T CurrentClientConnection<T>(this AbpHub<T> hub) where T : class
    {
        return hub.Clients.Client((hub.Context.ConnectionId));
    }
}