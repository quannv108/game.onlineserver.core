using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.Inventory.Managers;

public interface IInventoryItemManager : IDomainService
{
    Task<InventoryItem> GetAsync(long itemId);
}