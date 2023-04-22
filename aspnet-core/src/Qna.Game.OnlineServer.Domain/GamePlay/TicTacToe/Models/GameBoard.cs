using System.Collections.Generic;

namespace Qna.Game.OnlineServer.GamePlay.TicTacToe.Models;

public class GameBoard
{
    public int Size { get; }
    public Mark[,] Marks { get; }

    public GameBoard(int size)
    {
        Size = size;
        Marks = new Mark[size, size];
    }
}

public static class GameBoardExtensions {
    
    public static List<List<char>> GetMarksAsList(this GameBoard board)
    {
        List<List<char>> l = new();
        for (var row = 0; row < board.Size; row++)
        {
            var ll = new List<char>();
            l.Add(ll);
            for (var column = 0; column < board.Size; column++)
            {
                ll.Add(board.Marks[row, column].ToChar());
            }
        }

        return l;
    }
}