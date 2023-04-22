using Qna.Game.OnlineServer.Game;

namespace Qna.Game.OnlineServer.Room.Helpers;

public static class RoomExtensions
{
    public static bool CanJoinForPlay(this Room room, int totalPlayerToJoin)
    {
        return room.TotalCurrentPlayers + totalPlayerToJoin <= room.MaxPlayablePlayer
               && room.State == RoomState.Matching;
    }

    public static bool IsEnoughForStartGame(this Room room)
    {
        return room.TotalCurrentPlayers == room.MaxPlayablePlayer;
    }

    public static string GetRoomName(this Room room)
    {
        return $"{room.GameId}_{room.Id}";
    }
}