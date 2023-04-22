using Qna.Game.OnlineServer.SignalR.Contracts.Match;
using Qna.Game.OnlineServer.SignalR.Contracts.Match.Events;

namespace Qna.Game.OnlineServer.SignalR.Contracts.Hub.Core;

public interface IMatchClientAction
{
    Task MatchFoundAsync(GameMatchDto gameMatchDto);
    Task UpdateMatchPlayersAsync(MatchPlayersUpdateEventDto eventDto);
    Task UpdateMatchStateAsync(MatchStateEventUpdateDto eventDto);
}