using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using Qna.Game.OnlineServer.Concurrency;
using Qna.Game.OnlineServer.Game;
using Qna.Game.OnlineServer.GamePlay;
using Qna.Game.OnlineServer.GamePlay.Players;
using Volo.Abp.Domain.Entities.Auditing;

namespace Qna.Game.OnlineServer.Room;

[NotMapped]
public sealed class Room : CreationAuditedEntity<Guid>
{
    public SynchronizeList<GamePlayer> Players { get; } = new();
    [JsonIgnore]
    public ImmutableList<Guid> PlayerIds => Players.ToList().Select(x => x.Id).ToImmutableList();
    public int TotalCurrentPlayers => Players.Count;
    public int MaxPlayablePlayer { get; set; }

    public RoomState State { get; internal set; }
    
    public long GameId { get; set; }
    
    
    [JsonIgnore]
    public IGameLoop GameLoop { get; set; }
    
    public string ConditionKey { get; set; }

    internal Room()
    {
        CreationTime = DateTime.UtcNow;
        Id = Guid.NewGuid();
    }
}