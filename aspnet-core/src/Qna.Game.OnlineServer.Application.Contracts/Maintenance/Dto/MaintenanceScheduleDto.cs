using System;

namespace Qna.Game.OnlineServer.Maintenance.Dto;

public class MaintenanceScheduleDto
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public MaintenanceModule Module { get; set; }
    public string Message { get; set; }
}