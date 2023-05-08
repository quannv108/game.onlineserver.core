using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Qna.Game.OnlineServer.Schema;
using Volo.Abp.Identity;

namespace Qna.Game.OnlineServer.Leaderboard;

public class ScoreT<T> where T : class, ILeaderboard
{   
    public decimal Score { get; set; }
}

public abstract class UserScore<T> : ScoreT<T> where T : class, ILeaderboard
{
    [Key]
    public Guid UserId { get; set; }
    public IdentityUser User { get; set; }
}

public abstract class PlayerScore<T> : ScoreT<T> where T : class, ILeaderboard
{
    [Key]
    public long PlayerId { get; set; }
}

[Table("UserOverall", Schema = SchemaNames.Leaderboard)]
public class OverallUserScore : UserScore<UserOverallLeaderboard>
{
    
}

public class UserScoreResult : UserScore<ILeaderboard>
{
    [NotMapped]
    public string DisplayName { get; set; }
    [NotMapped]
    public string DisplayRank { get; set; }
}