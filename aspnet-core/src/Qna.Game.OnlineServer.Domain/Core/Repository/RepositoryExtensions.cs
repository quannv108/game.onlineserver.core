using System.Linq;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Qna.Game.OnlineServer.Core.Repository;

public static class RepositoryExtensions
{
    public static IQueryable<TEntity> GetAll<TEntity, TEntityKey>(this IRepository<TEntity, TEntityKey> repository)
    where TEntity : Entity<TEntityKey>
    {
        var queryable = repository.GetQueryableAsync().Result;
        return queryable;
    }
}