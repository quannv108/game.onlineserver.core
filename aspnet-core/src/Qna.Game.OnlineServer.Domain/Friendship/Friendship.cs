using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;

namespace Qna.Game.OnlineServer.Friendship;

[NotMapped]
public class Friendship : Entity<Guid>
{
    public Friendship(Guid invitationId) : base(invitationId){}

    public Guid FriendUserId { get; set; }
    public IdentityUser FriendUser { get; set; }
    public DateTime StartTime { get; set; }
}