using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Qna.Game.OnlineServer.Game;
using Qna.Game.OnlineServer.InGame;
using Qna.Game.OnlineServer.Session;
using Volo.Abp.Domain.Entities.Auditing;

namespace Qna.Game.OnlineServer.Room;

[NotMapped]
public sealed class Room : CreationAuditedEntity<Guid>
{
    public List<GamePlayer> Players { get; } = new();
    public int TotalCurrentPlayers => Players.Count;
    public int MaxPlayablePlayer { get; set; }

    public RoomState State { get; internal set; }

    internal Room(UserConnectionSession first)
    {
        CreationTime = DateTime.UtcNow;
        CreatorId = first.UserId;
        Id = Guid.NewGuid();
        
        Players.Add(first.CurrentPlayer);
    }
}