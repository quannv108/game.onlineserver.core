using System;

namespace Qna.Game.OnlineServer.Room.Events;

public class RoomPlayerAddedEvent
{
    public Guid RoomId { get; set; }
    public string NewPlayerConnectionId { get; set; }
}