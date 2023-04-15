using Qna.Game.OnlineServer.Session;
using Qna.Game.OnlineServer.SignalR.Session;
using Volo.Abp.AspNetCore.SignalR;

namespace Qna.Game.OnlineServer.SignalR.Hub.Core;

public abstract class BaseHub<T> : AbpHub<T> where T : class
{
    protected UserConnectionSession CurrentUserConnectionSession => SessionService.Find(CurrentUser.Id!.Value);
    protected T CallerClient => Clients.Client(Context.ConnectionId);

    protected ISessionService SessionService => LazyServiceProvider.LazyGetRequiredService<SessionService>();
}