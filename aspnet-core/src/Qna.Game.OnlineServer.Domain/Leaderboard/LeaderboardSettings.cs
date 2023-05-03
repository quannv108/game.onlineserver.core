using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Qna.Game.OnlineServer.Schema;
using Volo.Abp.Domain.Entities;

namespace Qna.Game.OnlineServer.Leaderboard;

[NotMapped]
public class LeaderboardSettings
{
    public LeaderboardType Type { get; set; }
    public List<LeaderboardRankRange> RankRanges { get; set; }
}

[NotMapped]
[Table("RankRange", Schema = SchemaNames.Leaderboard)]
public class LeaderboardRankRange : Entity<long>
{
    public LeaderboardType Type { get; set; }
    public int RangeStep { get; }
    public int RankFrom { get; }
    public int RankTo { get; }

    public LeaderboardRankRange(int from, int to, int step)
    {
        if ((from - to - 1) % step != 0)
        {
            throw new NotSupportedException($"invalid step {step} in range {from}-{to}");
        }

        RankFrom = from;
        RankTo = to;
        RangeStep = step;
    }
}

public static class DefaultLeaderboardRankRange
{
    public static List<LeaderboardRankRange> DefaultRankRanges = new List<LeaderboardRankRange>
    {
        new LeaderboardRankRange(1, 10, 1), // 10
        new LeaderboardRankRange(11, 100, 10), // 9
        new LeaderboardRankRange(101, 1000, 100), // 9
        new LeaderboardRankRange(1001, 10000, 1000), // 9
        new LeaderboardRankRange(10001, 100000, 10000), // 9
        new LeaderboardRankRange(100001, 1000000, 100000), // 9
        new LeaderboardRankRange(1000001, 10000000, 1000000) // 9
    };
}