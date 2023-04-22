namespace Qna.Game.OnlineServer.GamePlay.TicTacToe.Models.UserActions;

public class UserPlacedMarkAction
{
    public int RowIndex { get; set; }
    public int ColumnIndex { get; set; }
    public Mark Mark { get; set; }
}