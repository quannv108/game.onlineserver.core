using System;

namespace Qna.Game.OnlineServer.GamePlay.TicTacToe.Models;

public enum Mark
{
    None = 0,
    X,
    O
}

public static class MarkExtensions
{
    public static char ToChar(this Mark mark)
    {
        return mark switch
        {
            Mark.None => '_',
            Mark.X => 'X',
            Mark.O => 'O',
            _ => throw new ArgumentOutOfRangeException(nameof(mark), mark, null)
        };
    }
}