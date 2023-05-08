using System;
using System.ComponentModel.DataAnnotations.Schema;
using Qna.Game.OnlineServer.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Qna.Game.OnlineServer.GamePlay.Players;

[Table("GamePlayer", Schema = SchemaNames.Game)]
public class GamePlayer : FullAuditedEntity<Guid>
{
    public Guid UserId { get; set; }
    
    public long GameId { get; set; }
    public Game.Game Game { get; set; }
    
    public int CurrentLevel { get; set; }
}