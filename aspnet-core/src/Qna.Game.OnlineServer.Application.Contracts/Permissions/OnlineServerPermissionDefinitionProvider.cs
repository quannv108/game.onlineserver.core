using Qna.Game.OnlineServer.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Qna.Game.OnlineServer.Permissions;

public class OnlineServerPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(OnlineServerPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(OnlineServerPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<OnlineServerResource>(name);
    }
}
