namespace Qna.Game.OnlineServer.Leaderboard;

public interface ILeaderboard
{
    public LeaderboardType Type { get; }
    public string Name { get; }
}

public class UserOverallLeaderboard : ILeaderboard
{
    public LeaderboardType Type => LeaderboardType.UserOverall;
    public string Name => nameof(LeaderboardType.UserOverall);
}