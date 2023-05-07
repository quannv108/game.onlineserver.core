using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using DotLiquid;
using Volo.Abp;

namespace Qna.Game.OnlineServer.Notifications.Handlers;

public class NotificationMessageNormalizer : INotificationMessageNormalizer
{
    public void Normalize(NotificationMessage message)
    {
        if (message.Type == NotificationMessageType.DirectContent)
        {
            return;
        }

        try
        {
            var parameters = JsonSerializer.Deserialize<JsonObject>(message.TemplateParameters);

            parameters.TryGetPropertyValue(nameof(ITemplateParameters.Title), out var titleParams);
            parameters.TryGetPropertyValue(nameof(ITemplateParameters.Content), out var contentParams);

            message.Title = RenderContent(message.Template.Title, titleParams!.AsObject());
            message.Content = RenderContent(message.Template.Content, contentParams!.AsObject());
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException($"unable to grab parameters of message {message.Id}", innerException: ex);
        }
    }

    private static string RenderContent(string templateInput, JsonObject parameters)
    {
        var hash = CreateHash(parameters);
        var template = Template.Parse(templateInput);
        return template.Render(hash);
    }

    private static Hash CreateHash(JsonObject parameters)
    {
        var hash = new Hash();
        using var enumerator = parameters.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var key = enumerator.Current.Key;
            var element = enumerator.Current.Value;
            if (element is null)
            {
                continue;
            }

            var jsonValue = element.AsValue();
            if (jsonValue.TryGetValue(out string strValue))
            {
                hash.Add(key, strValue);
            }
            else if (jsonValue.TryGetValue(out long longValue))
            {
                hash.Add(key, longValue);
            }
            else if (jsonValue.TryGetValue(out decimal doubleValue))
            {
                hash.Add(key, doubleValue);
            }
            else if (jsonValue.TryGetValue(out bool boolValue))
            {
                hash.Add(key, boolValue);
            }
        }

        return hash;
    }
}