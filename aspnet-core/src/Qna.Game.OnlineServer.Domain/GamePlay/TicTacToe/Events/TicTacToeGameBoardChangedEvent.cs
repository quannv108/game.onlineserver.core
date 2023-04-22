using Qna.Game.OnlineServer.GamePlay.Events;
using Qna.Game.OnlineServer.GamePlay.TicTacToe.Models;

namespace Qna.Game.OnlineServer.GamePlay.TicTacToe.Events;

public class TicTacToeGameBoardChangedEvent : IGameUpdateEvent
{
    public GameBoard Board { get; set; }
    public Mark CurrentTurnMark { get; set; }
    public string RoomName { get; set; }
}