using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Qna.Game.OnlineServer.Data;

/* This is used if database provider does't define
 * IOnlineServerDbSchemaMigrator implementation.
 */
public class NullOnlineServerDbSchemaMigrator : IOnlineServerDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
