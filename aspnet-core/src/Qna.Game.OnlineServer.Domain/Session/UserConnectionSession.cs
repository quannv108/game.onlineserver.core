using System;
using System.ComponentModel.DataAnnotations.Schema;
using Qna.Game.OnlineServer.GamePlay;
using Qna.Game.OnlineServer.GamePlay.Players;

namespace Qna.Game.OnlineServer.Session;

[NotMapped]
public class UserConnectionSession
{
    public Guid UserId { get; set; }
    public string ConnectionId { get; set; }
    
    public GamePlayer CurrentPlayer { get; set; }
    public Room.Room CurrentMatch { get; set; }

    internal UserConnectionSession(Guid userId, string connectionId)
    {
        UserId = userId;
        ConnectionId = connectionId;
    }
}