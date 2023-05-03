using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace Qna.Game.OnlineServer.Leaderboard;

[NotMapped]
public class Leaderboard
{
    public LeaderboardType Type { get; set; }
    public LeaderboardSettings Settings { get; set; }
    [NotMapped]
    public List<PlayerScoreResult> Scores { get; set; }
}

public class LeaderboardRankRangeResult : Entity<long>
{
    public long RankRangeId { get; set; }
    public LeaderboardRankRange RankRange { get; set; }
    public int RankFrom { get; set; }
    public decimal ScoreFrom { get; set; }
}