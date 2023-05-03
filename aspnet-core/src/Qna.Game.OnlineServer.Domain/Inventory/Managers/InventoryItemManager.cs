using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Qna.Game.OnlineServer.Core.Repository;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.Inventory.Managers;

public class InventoryItemManager : DomainService, IInventoryItemManager
{
    private readonly IRepository<InventoryItem, long> _inventoryItemRepository;

    public InventoryItemManager(IRepository<InventoryItem, long> inventoryItemRepository)
    {
        _inventoryItemRepository = inventoryItemRepository;
    }
    public Task<InventoryItem> GetAsync(long itemId)
    {
        return _inventoryItemRepository.GetAll().AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == itemId);
    }
}