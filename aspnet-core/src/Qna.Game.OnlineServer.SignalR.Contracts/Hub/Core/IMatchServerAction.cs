using Qna.Game.OnlineServer.SignalR.Contracts.Match;

namespace Qna.Game.OnlineServer.SignalR.Contracts.Hub.Core;

public interface IMatchServerAction
{
    Task AutoJoinMatchAsync(AutoJoinMatchInput input);
    Task LeaveMatchAsync();
}