using System;

namespace Qna.Game.OnlineServer.Room.Events;

public class RoomPlayerRemovedEvent
{
    public Guid RoomId { get; set; }
    public string RemovedPlayerConnectionId { get; set; }
}