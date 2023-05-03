using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.Maintenance.Managers;

public interface IMaintenanceScheduleManager : IDomainService
{
    Task<List<MaintenanceSchedule>> GetAllAsync();
    Task<bool> IsSignalROnMaintenanceAsync();
}