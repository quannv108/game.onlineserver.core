using System;
using Volo.Abp.Users;

namespace Qna.Game.OnlineServer.Core.Users;

public static class ICurrentUserExtensions
{
    public static Guid GetUserId(this ICurrentUser currentUser)
    {
        if (currentUser.Id == null)
        {
            throw new UnauthorizedAccessException("userId not existing");
        }

        return currentUser.Id.Value;
    }
}