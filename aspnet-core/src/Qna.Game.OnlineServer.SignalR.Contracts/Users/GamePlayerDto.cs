namespace Qna.Game.OnlineServer.SignalR.Contracts.Users;

public class GamePlayerDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    public int CurrentLevel { get; set; }
}