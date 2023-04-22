using System;
using System.Collections.Immutable;
using Qna.Game.OnlineServer.GamePlay.Events;
using Qna.Game.OnlineServer.GamePlay.TicTacToe.Models;

namespace Qna.Game.OnlineServer.GamePlay.TicTacToe.Events;

public class TicTacToeGameStartEvent : IGameStartEvent
{
    public Guid GamePlayId { get; set; }
    public string RoomName { get; set; }
    public ImmutableDictionary<Mark, Guid> MarkPlayerIds { get; set; }
    public Mark CurrentTurnMark { get; set; }
}