using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Qna.Game.OnlineServer.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Qna.Game.OnlineServer.Leaderboard;

public abstract class PlayerScoreBase : AuditedEntity<Guid>
{
    public Guid UserId { get; set; }
    public decimal Score { get; set; }
    public int Rank { get; set; }
}

public class PlayerScoreResult : PlayerScoreBase
{
    public string DisplayName { get; set; }
    public string DisplayRank { get; set; }
}

[NotMapped]
[Table("UserOverallScore", Schema=SchemaNames.Leaderboard)]
[Index(nameof(UserId), IsUnique = true)]
public class OverallPlayerScore : PlayerScoreBase
{
    
}