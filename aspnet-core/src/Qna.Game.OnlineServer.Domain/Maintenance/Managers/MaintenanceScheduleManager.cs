using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Qna.Game.OnlineServer.Core.Repository;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.Maintenance.Managers;

public class MaintenanceScheduleManager : DomainService, IMaintenanceScheduleManager
{
    private readonly IRepository<MaintenanceSchedule, long> _maintenanceScheduleRepository;

    public MaintenanceScheduleManager(IRepository<MaintenanceSchedule, long> maintenanceScheduleRepository)
    {
        _maintenanceScheduleRepository = maintenanceScheduleRepository;
    }
    
    public Task<List<MaintenanceSchedule>> GetAllAsync()
    {
        var currentTime = DateTime.UtcNow;
        return _maintenanceScheduleRepository.GetAll().AsNoTracking()
            .Where(x => x.EndTime > currentTime)
            .ToListAsync();
    }

    public async Task<bool> IsSignalROnMaintenanceAsync()
    {
        var currentTime = DateTime.UtcNow;
        var maintenances = await GetAllAsync();
        return maintenances.Any(m =>
            currentTime >= m.StartTime
            && currentTime <= m.EndTime
            && m.Module == MaintenanceModule.All);
    }
}