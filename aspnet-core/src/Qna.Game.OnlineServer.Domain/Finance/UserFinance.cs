using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Qna.Game.OnlineServer.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Qna.Game.OnlineServer.Finance;

[NotMapped]
[Table("UserFinance", Schema = SchemaNames.Game)]
[Index(nameof(UserId), IsUnique = true)]
public class UserFinance : FullAuditedEntity<Guid>
{
    public Guid UserId { get; set; }
    
    public long Gold { get; set; }
}