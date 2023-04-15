using Qna.Game.OnlineServer.Game;

namespace Qna.Game.OnlineServer.SignalR.Contracts.Match.Events;

public class MatchStateEventUpdateDto
{
    public RoomState State { get; set; }
}