using Qna.Game.OnlineServer.SignalR.Contracts.Match;
using Qna.Game.OnlineServer.SignalR.Contracts.Match.Events;

namespace Qna.Game.OnlineServer.SignalR.Contracts.Hub.Main;

/// <summary>
/// interface to show which action server can call to client
/// </summary>
public interface IMainHubServerToClient
{
    Task MultiLoginDetectedAsync();
    Task ShowErrorAsync(string errorMessage);
    Task HiAsync();
    Task MatchFoundAsync(GameMatchDto gameMatchDto);
    Task UpdateMatchPlayersAsync(MatchPlayersUpdateEventDto eventDto);
    Task UpdateMatchStateAsync(MatchStateEventUpdateDto eventDto);
}