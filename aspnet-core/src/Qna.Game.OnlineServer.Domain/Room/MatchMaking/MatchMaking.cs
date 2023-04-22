using System.Linq;
using System.Threading.Tasks;
using Qna.Game.OnlineServer.GamePlay;
using Qna.Game.OnlineServer.GamePlay.Players;
using Qna.Game.OnlineServer.Room.Helpers;
using Qna.Game.OnlineServer.Room.Storage;
using Qna.Game.OnlineServer.Session;

namespace Qna.Game.OnlineServer.Room.MatchMaking;

public class MatchMaking : IMatchMaking
{
    private readonly IRoomStorage _roomStorage;

    public MatchMaking(IRoomStorage roomStorage)
    {
        _roomStorage = roomStorage;
    }

    public Task<(string, Room)> FindMatchAsync(UserConnectionSession userConnectionSession, long gameId)
    {
        var conditionKey = GetConditionKey(userConnectionSession.CurrentPlayer, gameId);

        var joinableMatchs = _roomStorage.GetAll(conditionKey)
            .Where(x => x.CanJoinForPlay(1))
            .OrderByDescending(x => x.TotalCurrentPlayers);
        return Task.FromResult((conditionKey, joinableMatchs.FirstOrDefault()));
    }

    public string GetConditionKey(GamePlayer player, long gameId)
    {
        return $"{gameId}_{player.CurrentLevel.ToString()}";
    }
}