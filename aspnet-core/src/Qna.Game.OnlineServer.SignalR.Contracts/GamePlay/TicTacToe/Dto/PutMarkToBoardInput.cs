namespace Qna.Game.OnlineServer.SignalR.Contracts.GamePlay.TicTacToe.Dto;

public class PutMarkToBoardInput
{
    public int RowIndex { get; set; }
    public int ColumnIndex { get; set; }
}