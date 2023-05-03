using Qna.Game.OnlineServer.Game;
using Qna.Game.OnlineServer.Match;
using Qna.Game.OnlineServer.SignalR.Contracts.Users;

namespace Qna.Game.OnlineServer.SignalR.Contracts.Match;

public class GameMatchDto
{
    public Guid Id { get; set; }
    
    public List<GamePlayerDto> Players { get; set; }
    
    public RoomState State { get; set; }
}