using Qna.Game.OnlineServer.SignalR.Contracts.Users;

namespace Qna.Game.OnlineServer.SignalR.Contracts.Match.Events;

public class MatchPlayersUpdateEventDto
{
    public List<GamePlayerDto> Players { get; set; }
}