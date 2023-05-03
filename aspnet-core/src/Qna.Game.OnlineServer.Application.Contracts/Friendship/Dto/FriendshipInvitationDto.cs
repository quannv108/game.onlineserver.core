using System;
using Volo.Abp.Application.Dtos;

namespace Qna.Game.OnlineServer.Friendship.Dto;

public class FriendshipInvitationDto : EntityDto<Guid>
{
    public Guid FromUserId { get; set; }
    public FriendUserDto FromUser { get; set; }
    public DateTime RequestTime { get; set; }
}