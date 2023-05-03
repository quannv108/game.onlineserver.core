using System;

namespace Qna.Game.OnlineServer.Friendship.Dto;

public class AnswerFriendshipRequestInput
{
    public Guid RequestId { get; set; }
    public FriendshipRequestActionType ActionType { get; set; }
}