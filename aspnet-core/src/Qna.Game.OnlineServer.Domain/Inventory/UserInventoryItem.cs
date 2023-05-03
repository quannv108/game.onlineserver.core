using System;
using System.ComponentModel.DataAnnotations.Schema;
using Qna.Game.OnlineServer.Schema;
using Volo.Abp.Domain.Entities;

namespace Qna.Game.OnlineServer.Inventory;

[NotMapped]
[Table("UserInventoryItem", Schema = SchemaNames.Game)]
public class UserInventoryItem : Entity<Guid>
{
    public Guid UserId { get; set; }
    
    public long ItemId { get; set; }
    public InventoryItem Item { get; set; }
    
    public int Count { get; set; }
}