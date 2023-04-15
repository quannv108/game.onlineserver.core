using System;

namespace Qna.Game.OnlineServer.Room.Events;

public class RoomCreatedEvent
{
    public Guid RoomId { get; set; }
    public string RoomName { get; set; }
}