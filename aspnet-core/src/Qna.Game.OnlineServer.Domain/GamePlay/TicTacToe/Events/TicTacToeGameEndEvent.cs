using System;
using Qna.Game.OnlineServer.GamePlay.Events;
using Qna.Game.OnlineServer.GamePlay.TicTacToe.Models;

namespace Qna.Game.OnlineServer.GamePlay.TicTacToe.Events;

public class TicTacToeGameEndEvent : IGameEndEvent
{
    public Mark Winner { get; set; }
    public Guid RoomId { get; set; }
    public string RoomName { get; set; }
}