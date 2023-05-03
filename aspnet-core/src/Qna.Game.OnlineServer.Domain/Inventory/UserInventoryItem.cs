using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Qna.Game.OnlineServer.Schema;
using Volo.Abp.Domain.Entities;

namespace Qna.Game.OnlineServer.Inventory;

[NotMapped]
[Table("UserInventoryItem", Schema = SchemaNames.Game)]
[Index(nameof(UserId))]
[Index(nameof(ItemId))]
public class UserInventoryItem : Entity<Guid>
{
    public Guid UserId { get; set; }
    public Guid ItemId { get; set; }
    [NotMapped]
    public InventoryItem Item { get; set; }
    public int Count { get; set; }
}