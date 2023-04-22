namespace Qna.Game.OnlineServer.SignalR.Contracts.GamePlay.TicTacToe.Dto;

public class TicTacToeBoardDto
{
    public List<List<char>> Marks { get; set; }
    public char CurrentTurnMark { get; set; }
}