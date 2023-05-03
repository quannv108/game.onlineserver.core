using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Qna.Game.OnlineServer.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace Qna.Game.OnlineServer.Friendship;

[Table("Invitation", Schema = SchemaNames.Friendship)]
public class FriendshipInvitation : FullAuditedEntity<Guid>
{
    public Guid FromUserId { get; set; }
    public IdentityUser FromUser { get; set; }
    public Guid ToUserId { get; set; }
    public IdentityUser ToUser { get; set; }
    
    [MaxLength(FriendshipInvitationConsts.MessageMaxLength)]
    public string Message { get; set; }
    public FriendRequestInvitationStatus Status { get; set; }
}