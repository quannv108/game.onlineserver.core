using Microsoft.AspNetCore.SignalR;
using Volo.Abp.Application.Services;

namespace Qna.Game.OnlineServer.SignalR.Hub.Core;

public abstract class SignalRBaseAppService<THub, TClient> : ApplicationService
    where THub : Hub<TClient>
    where TClient : class
{
    protected IGroupManager Groups => HubContext.Groups;
    protected IHubClients<TClient> Clients => HubContext.Clients;

    protected IHubContext<THub, TClient> HubContext =>
        LazyServiceProvider.LazyGetRequiredService<IHubContext<THub, TClient>>();

    protected TClient GetClient(string connectionId) => Clients.Client(connectionId);
}