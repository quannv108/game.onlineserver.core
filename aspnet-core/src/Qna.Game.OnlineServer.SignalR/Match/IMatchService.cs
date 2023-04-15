using Qna.Game.OnlineServer.Session;
using Volo.Abp.Application.Services;

namespace Qna.Game.OnlineServer.SignalR.Match;

public interface IMatchService : IApplicationService
{
    Task<Room.Room> AutoJoinAsync(UserConnectionSession session);
}