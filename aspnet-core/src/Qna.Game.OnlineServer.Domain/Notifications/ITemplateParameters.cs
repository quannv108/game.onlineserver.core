namespace Qna.Game.OnlineServer.Notifications;

public interface ITemplateParameters
{
    object Title { get; }
    object Content { get; }
}