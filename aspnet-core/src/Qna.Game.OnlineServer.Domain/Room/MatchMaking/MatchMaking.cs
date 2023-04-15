using System.Linq;
using System.Threading.Tasks;
using Qna.Game.OnlineServer.InGame;
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

    public Task<(string, Room)> FindMatchAsync(UserConnectionSession userConnectionSession)
    {
        var conditionKey = GetConditionKey(userConnectionSession.CurrentPlayer);

        var joinableMatchs = _roomStorage.GetAll(conditionKey)
            .Where(x => x.CanJoinForPlay(1))
            .OrderByDescending(x => x.TotalCurrentPlayers);
        return Task.FromResult((conditionKey, joinableMatchs.FirstOrDefault()));
    }

    public string GetConditionKey(Room room)
    {
        return GetConditionKey(room.Players.First());
    }

    private static string GetConditionKey(GamePlayer player)
    {
        return player.CurrentLevel.ToString();
    }
}