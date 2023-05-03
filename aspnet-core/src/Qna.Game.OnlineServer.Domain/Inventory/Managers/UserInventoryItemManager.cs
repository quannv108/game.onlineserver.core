using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Qna.Game.OnlineServer.Core.Repository;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.Inventory.Managers;

public class UserInventoryItemManager : DomainService, IUserInventoryItemManager
{
    private readonly IRepository<UserInventoryItem, Guid> _userInventoryItemRepository;

    public UserInventoryItemManager(IRepository<UserInventoryItem, Guid> userInventoryItemRepository)
    {
        _userInventoryItemRepository = userInventoryItemRepository;
    }
    
    public Task AddAsync(Guid userId, InventoryItem inventoryItem, int count)
    {
        var userInventoryItem = new UserInventoryItem
        {
            ItemId = inventoryItem.Id,
            UserId = userId,
            Count = count
        };
        return _userInventoryItemRepository.InsertAsync(userInventoryItem);
    }
    
    public Task UpdateCountAsync(UserInventoryItem item)
    {
        return _userInventoryItemRepository.UpdateAsync(item);
    }
    
    public Task<List<UserInventoryItem>> GetAllAsync(Guid userId, InventoryItemType type)
    {
        return _userInventoryItemRepository.GetAll().AsNoTracking()
            .Include(x => x.Item)
            .Where(x => x.Item.Type == type && x.UserId == userId && x.Count > 0)
            .ToListAsync();
    }
}