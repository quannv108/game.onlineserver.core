using Volo.Abp.Settings;

namespace Qna.Game.OnlineServer.Settings;

public class OnlineServerSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(OnlineServerSettings.MySetting1));
    }
}
