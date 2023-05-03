using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace Qna.Game.OnlineServer.Inventory;

[NotMapped]
public class InventoryItem : Entity<Guid>
{
    public string Name { get; set; }
    public InventoryItemType Type { get; set; }
}