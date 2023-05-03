using System.Collections.Generic;
using System.Threading.Tasks;
using Qna.Game.OnlineServer.Maintenance.Dto;
using Volo.Abp.Application.Services;

namespace Qna.Game.OnlineServer.Maintenance;

public interface IMaintenanceScheduleAppService : IApplicationService
{
    Task<List<MaintenanceScheduleDto>> GetAllAsync();
}