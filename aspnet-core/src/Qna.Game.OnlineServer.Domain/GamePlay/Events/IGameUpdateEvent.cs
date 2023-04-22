namespace Qna.Game.OnlineServer.GamePlay.Events;

public interface IGameUpdateEvent
{
    string RoomName { get; set; }
    
}