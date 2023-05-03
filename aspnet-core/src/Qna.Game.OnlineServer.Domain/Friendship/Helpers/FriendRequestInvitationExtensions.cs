using System;

namespace Qna.Game.OnlineServer.Friendship.Helpers;

public static class FriendRequestInvitationExtensions
{
    public static Friendship ToFriendship(this FriendshipInvitation invitation, Guid userId)
    {
        if (invitation.FromUserId == userId)
        {
            return new Friendship(invitation.Id)
            {
                FriendUserId = invitation.ToUserId,
                FriendUser = invitation.ToUser,
                StartTime = invitation.LastModificationTime.Value
            };
        }else if (invitation.ToUserId == userId)
        {
            return new Friendship(invitation.Id)
            {
                FriendUserId = invitation.FromUserId,
                FriendUser = invitation.FromUser,
                StartTime = invitation.LastModificationTime.Value
            };
        }
        else
        {
            throw new NotSupportedException("invalid convert");
        }
    }
}