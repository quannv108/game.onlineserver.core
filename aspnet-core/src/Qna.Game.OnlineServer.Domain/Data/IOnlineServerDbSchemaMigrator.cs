using System.Threading.Tasks;

namespace Qna.Game.OnlineServer.Data;

public interface IOnlineServerDbSchemaMigrator
{
    Task MigrateAsync();
}
