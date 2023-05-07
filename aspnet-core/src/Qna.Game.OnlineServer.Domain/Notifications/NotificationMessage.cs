using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Qna.Game.OnlineServer.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace Qna.Game.OnlineServer.Notifications;

[Table("NotificationMessage", Schema = SchemaNames.Notification)]
public class NotificationMessage : FullAuditedEntity<Guid>
{
    public Guid UserId { get; set; }
    public IdentityUser User { get; set; }
    
    public NotificationMessageType Type { get; set; }
    
    public bool IsRead { get; set; }
    
    [MaxLength(NotificationMessageConsts.TitleMaxLength)]
    public string Title { get; set; }
    
    [MaxLength(NotificationMessageConsts.ContentMaxLength)]
    public string Content { get; set; }
    
    public long? TemplateId { get; set; }
    public NotificationMessageTemplate Template { get; set; }
    
    [MaxLength(NotificationMessageConsts.TemplateParametersMaxLength)]
    public string TemplateParameters { get; set; }
}