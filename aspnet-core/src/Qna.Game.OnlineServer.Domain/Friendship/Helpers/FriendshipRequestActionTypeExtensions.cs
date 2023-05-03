using System;

namespace Qna.Game.OnlineServer.Friendship.Helpers;

public static class FriendshipRequestActionTypeExtensions
{
    public static FriendRequestInvitationStatus ToInvitationStatus(this FriendshipRequestActionType actionType)
    {
        return actionType switch
        {
            FriendshipRequestActionType.Ignore => FriendRequestInvitationStatus.Rejected,
            FriendshipRequestActionType.Accept => FriendRequestInvitationStatus.Accepted,
            FriendshipRequestActionType.Block => FriendRequestInvitationStatus.Blocked,
            _ => throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null)
        };
    }
}