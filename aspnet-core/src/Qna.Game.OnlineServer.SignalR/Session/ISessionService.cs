using Qna.Game.OnlineServer.Session;
using Volo.Abp.Application.Services;

namespace Qna.Game.OnlineServer.SignalR.Session;

public interface ISessionService : IApplicationService
{
    UserConnectionSession Find(Guid userId);
    Task AddAsync(Guid userId, string connectionId);
    void Remove(Guid userId, string connectionId);
}