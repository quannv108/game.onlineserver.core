using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.Inventory.Managers;

public interface IUserInventoryItemManager : IDomainService
{
    Task AddAsync(Guid userId, InventoryItem inventoryItem, int count);
    Task UpdateCountAsync(UserInventoryItem item);
    Task<List<UserInventoryItem>> GetAllAsync(Guid userId, InventoryItemType type);
}