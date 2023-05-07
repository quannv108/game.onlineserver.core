using System;
using System.Linq;
using Volo.Abp.Identity;

namespace Qna.Game.OnlineServer.Core.Users;

public static class IdentityUserExtensions
{
    public static IQueryable<IdentityUser> GetAllActiveUsers(this IQueryable<IdentityUser> queryable)
    {
        return queryable.Where(u => u.EmailConfirmed && (u.LockoutEnd == null || u.LockoutEnd < DateTime.UtcNow));
    }
}