using System;

namespace Qna.Game.OnlineServer.Friendship.Dto;

public class RequestFriendshipInput
{
    public Guid ToUserId { get; set; }
    public string Message { get; set; }
}