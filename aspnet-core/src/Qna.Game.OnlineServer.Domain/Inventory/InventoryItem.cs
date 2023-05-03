using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Qna.Game.OnlineServer.Schema;
using Volo.Abp.Domain.Entities;

namespace Qna.Game.OnlineServer.Inventory;

[NotMapped]
[Table("InventoryItem", Schema= SchemaNames.Game)]
public class InventoryItem : Entity<long>
{
    [MaxLength(InventoryItemConsts.NameMaxLength)]
    public string Name { get; set; }
    
    [MaxLength(InventoryItemConsts.DescriptionMaxLength)]
    public string Description { get; set; }
    public InventoryItemType Type { get; set; }
}