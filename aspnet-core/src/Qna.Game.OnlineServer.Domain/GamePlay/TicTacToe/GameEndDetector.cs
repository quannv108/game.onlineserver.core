using System.Collections.Generic;
using System.Linq;
using Qna.Game.OnlineServer.GamePlay.TicTacToe.Models;

namespace Qna.Game.OnlineServer.GamePlay.TicTacToe;

public class GameEndDetector : IGameEndDetector
{
    public (bool, Mark) ShouldEndGame(GameBoard board)
    {
        var mark = DetectWinner(board);
        return (mark != Mark.None, mark);
    }
    
    private static Mark DetectWinner(GameBoard board)
    {
        var row1 = IsRowSameMark(board, 0);
        var row2 = IsRowSameMark(board,1);
        var row3 = IsRowSameMark(board,2);
        var col1 = IsColumnSameMark(board,0);
        var col2 = IsColumnSameMark(board,1);
        var col3 = IsColumnSameMark(board,2);
        var diagonal1 = IsDiagonalFromBottomLeftToTopRightSameMark(board);
        var diagonal2 = IsDiagonalFromTopLeftToBottomRightSameMark(board);

        var l = new List<Mark> { row1, row2, row3, col1, col2, col3, diagonal1, diagonal2 };
        var winner = l.FirstOrDefault(x => x != Mark.None);
        return winner;
    }

    private static Mark IsRowSameMark(GameBoard board, int row)
    {
        var marks = board.Marks;
        var allMarksInRow = Enumerable.Range(0, board.Size)
            .Select(i => marks[row, i])
            .Distinct().ToList();
        return allMarksInRow.Count == 1 ? allMarksInRow.First() : Mark.None;
    }
    
    private static Mark IsColumnSameMark(GameBoard board, int col)
    {
        var marks = board.Marks;
        var allMarksInColumn = Enumerable.Range(0, board.Size)
            .Select(i => marks[i, col])
            .Distinct().ToList();
        return allMarksInColumn.Count == 1 ? allMarksInColumn.First() : Mark.None;
    }

    private static Mark IsDiagonalFromTopLeftToBottomRightSameMark(GameBoard board)
    {
        var allMarksInDiagonal = Enumerable.Range(0, board.Size)
            .Select(i => board.Marks[i, i])
            .Distinct().ToList();
        return allMarksInDiagonal.Count == 1 ? allMarksInDiagonal.First() : Mark.None;
    }
    
    private static Mark IsDiagonalFromBottomLeftToTopRightSameMark(GameBoard board)
    {
        var allMarksInDiagonal = Enumerable.Range(0, board.Size)
            .Select(i => board.Marks[i, board.Size - i - 1])
            .Distinct().ToList();
        return allMarksInDiagonal.Count == 1 ? allMarksInDiagonal.First() : Mark.None;
    }
}