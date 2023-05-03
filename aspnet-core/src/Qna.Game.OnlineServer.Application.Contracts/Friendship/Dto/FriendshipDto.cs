using System;

namespace Qna.Game.OnlineServer.Friendship.Dto;

public class FriendshipDto
{
    public Guid FriendUserId { get; set; }
    public FriendUserDto FriendUser { get; set; }
    public DateTime StartTime { get; set; }
}