namespace Qna.Game.OnlineServer.SignalR.Contracts.GamePlay.TicTacToe.Dto;

public class TicTacToeGameSetupDto
{
    public List<PlayerMarkDto> PlayerMarks { get; set; }
    public char StartTurnMark { get; set; }
}

public class PlayerMarkDto
{
    public Guid PlayerId { get; set; }
    public char Mark { get; set; }
}