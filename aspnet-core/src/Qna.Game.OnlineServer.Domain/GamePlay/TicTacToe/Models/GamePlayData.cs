using System;
using System.Collections.Immutable;
using System.Linq;

namespace Qna.Game.OnlineServer.GamePlay.TicTacToe.Models;

public class GamePlayData : IGamePlayData
{
    public Guid Id { get; set; }
    public GameBoard Board { get; set; }
    public Mark Winner { get; set; }
    public Mark CurrentTurnMark { get; set; }
    public ImmutableDictionary<Mark, Guid> MarkPlayerIds { get; set; }

}

public static class GamePlayDataExtensions
{
    public static Mark GetPlayerMark(this GamePlayData gamePlayData, Guid playerId)
    {
        return gamePlayData.MarkPlayerIds.FirstOrDefault(x => x.Value == playerId).Key;
    }
}

