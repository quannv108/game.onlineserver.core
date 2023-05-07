using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Qna.Game.OnlineServer.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Qna.Game.OnlineServer.Notifications;

[Table("NotificationMessageTemplate", Schema = SchemaNames.Notification)]
public class NotificationMessageTemplate : FullAuditedEntity<long>
{
    [MaxLength(NotificationMessageConsts.TitleMaxLength)]
    public string Title { get; set; }
    
    [MaxLength(NotificationMessageConsts.ContentMaxLength)]
    public string Content { get; set; }
}