using System.Threading.Tasks;
using Qna.Game.OnlineServer.Session;
using Volo.Abp.DependencyInjection;

namespace Qna.Game.OnlineServer.Room.MatchMaking;

public interface IMatchMaking : ITransientDependency
{
    Task<(string, Room)> FindMatchAsync(UserConnectionSession userConnectionSession);
    string GetConditionKey(Room room);
}