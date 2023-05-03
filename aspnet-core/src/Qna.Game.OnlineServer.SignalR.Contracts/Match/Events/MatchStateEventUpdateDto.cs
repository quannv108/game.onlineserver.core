using Qna.Game.OnlineServer.Game;
using Qna.Game.OnlineServer.Match;

namespace Qna.Game.OnlineServer.SignalR.Contracts.Match.Events;

public class MatchStateEventUpdateDto
{
    public RoomState State { get; set; }
}