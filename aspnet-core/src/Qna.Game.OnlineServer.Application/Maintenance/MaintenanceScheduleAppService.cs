using System.Collections.Generic;
using System.Threading.Tasks;
using Qna.Game.OnlineServer.Maintenance.Dto;
using Qna.Game.OnlineServer.Maintenance.Managers;

namespace Qna.Game.OnlineServer.Maintenance;

public class MaintenanceScheduleAppService : OnlineServerAppService, IMaintenanceScheduleAppService
{
    private readonly IMaintenanceScheduleManager _maintenanceScheduleManager;

    public MaintenanceScheduleAppService(IMaintenanceScheduleManager maintenanceScheduleManager)
    {
        _maintenanceScheduleManager = maintenanceScheduleManager;
    }

    public async Task<List<MaintenanceScheduleDto>> GetAllAsync()
    {
        var maintenances = await _maintenanceScheduleManager.GetAllAsync();
        return ObjectMapper.Map<List<MaintenanceSchedule>, List<MaintenanceScheduleDto>>(maintenances);
    }
}