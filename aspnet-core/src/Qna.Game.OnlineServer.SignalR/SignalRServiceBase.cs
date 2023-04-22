using Microsoft.AspNetCore.SignalR;
using Qna.Game.OnlineServer.SignalR.Contracts.Hub.Core;
using Volo.Abp.Application.Services;

namespace Qna.Game.OnlineServer.SignalR;

public abstract class SignalRServiceBase<THub, TClientAction> : ApplicationService
    where THub : Hub<TClientAction>
    where TClientAction : class, IHubClientActionBase
{
    protected IGroupManager Groups => HubContext.Groups;
    protected IHubClients<TClientAction> Clients => HubContext.Clients;

    protected IHubContext<THub, TClientAction> HubContext =>
        LazyServiceProvider.LazyGetRequiredService<IHubContext<THub, TClientAction>>();

    protected TClientAction GetClient(string connectionId) => Clients.Client(connectionId);
}