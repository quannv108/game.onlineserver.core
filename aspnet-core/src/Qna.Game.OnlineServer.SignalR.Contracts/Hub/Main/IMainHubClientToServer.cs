using Qna.Game.OnlineServer.SignalR.Contracts.Match;

namespace Qna.Game.OnlineServer.SignalR.Contracts.Hub.Main;

/// <summary>
/// interface to describe which action client can request to server
/// </summary>
public interface IMainHubClientToServer
{
    Task HelloAsync();
    Task AutoJoinMatchAsync(AutoJoinMatchInput input);
}