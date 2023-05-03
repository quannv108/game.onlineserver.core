using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Qna.Game.OnlineServer.Schema;
using Volo.Abp.Domain.Entities;

namespace Qna.Game.OnlineServer.Maintenance;

[Table("MaintenanceSchedule", Schema = SchemaNames.SystemConfig)]
public class MaintenanceSchedule : Entity<long>
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public MaintenanceModule Module { get; set; }
    
    [MaxLength(MaintenanceScheduleConsts.MessageMaxLength)]
    public string Message { get; set; }
}