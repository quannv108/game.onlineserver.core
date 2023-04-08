using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Qna.Game.OnlineServer.Data;
using Volo.Abp.DependencyInjection;

namespace Qna.Game.OnlineServer.EntityFrameworkCore;

public class EntityFrameworkCoreOnlineServerDbSchemaMigrator
    : IOnlineServerDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreOnlineServerDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the OnlineServerDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<OnlineServerDbContext>()
            .Database
            .MigrateAsync();
    }
}
