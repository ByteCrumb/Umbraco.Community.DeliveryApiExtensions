using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;

namespace UmbracoDeliveryApiExtensions.TestSite.Custom;

public class CreateUsersMigrationComposer : ComponentComposer<CreateUsersMigration>
{
}

public class CreateUsersMigration : IComponent
{
    private readonly ICoreScopeProvider _coreScopeProvider;
    private readonly IMigrationPlanExecutor _migrationPlanExecutor;
    private readonly IKeyValueService _keyValueService;
    private readonly IRuntimeState _runtimeState;

    public CreateUsersMigration(
        ICoreScopeProvider coreScopeProvider,
        IMigrationPlanExecutor migrationPlanExecutor,
        IKeyValueService keyValueService,
        IRuntimeState runtimeState)
    {
        _coreScopeProvider = coreScopeProvider;
        _migrationPlanExecutor = migrationPlanExecutor;
        _keyValueService = keyValueService;
        _runtimeState = runtimeState;
    }

    public void Initialize()
    {
        if (_runtimeState.Level < RuntimeLevel.Run)
        {
            return;
        }

        Upgrader upgrader = new(
            new MigrationPlan("CreateUsers")
                .From(string.Empty)
                .To<CreateDevAndEditorUsers>("CreateUsers-Migration"));
        upgrader.Execute(_migrationPlanExecutor, _coreScopeProvider, _keyValueService);
    }

    public void Terminate()
    {
    }
}

public class CreateDevAndEditorUsers : MigrationBase
{
    private readonly IUserService _userService;
    private readonly IOptions<UnattendedSettings> _unattendedSettings;
    private readonly IShortStringHelper _shortStringHelper;

    public CreateDevAndEditorUsers(
        IMigrationContext context,
        IUserService userService,
        IOptions<UnattendedSettings> unattendedSettings,
        IShortStringHelper shortStringHelper)
        : base(context)
    {
        _userService = userService;
        _unattendedSettings = unattendedSettings;
        _shortStringHelper = shortStringHelper;
    }

    protected override void Migrate()
    {
        string password = _unattendedSettings.Value.UnattendedUserPassword ?? "";

        // Get the editor user group
        UserGroup editorUserGroup = _userService.GetUserGroupByAlias(Constants.Security.EditorGroupAlias) as UserGroup
                                    ?? throw new InvalidOperationException("Could not find user group with alias 'editor'");

        // Create an editor user
        User editorUser = CreateUser("Editor", "editor@umbraco", password);
        editorUser.AddGroup(editorUserGroup);
        _userService.Save(editorUser);

        // Create a developer user group
        UserGroup developerUserGroup = new(_shortStringHelper)
        {
            Alias = "developer",
            Name = "Developers",
            StartContentId = -1,
            StartMediaId = -1,
            HasAccessToAllLanguages = true,
            Permissions = editorUserGroup.Permissions,
        };

        developerUserGroup.AddAllowedSection(Constants.Applications.Content);
        developerUserGroup.AddAllowedSection(Constants.Applications.Media);

        _userService.Save(developerUserGroup);

        // Create a developer user
        User developerUser = CreateUser("Developer", "dev@umbraco", password);
        developerUser.AddGroup(developerUserGroup);
        _userService.Save(developerUser);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5350:Do Not Use Weak Cryptographic Algorithms", Justification = "Not an issue. Ignore.")]
    private User CreateUser(string name, string email, string password)
    {
        HMACSHA1 hash = new() { Key = Encoding.Unicode.GetBytes(password) };
        string encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));

        User user = new(
            new GlobalSettings(),
            name,
            email,
            email,
            encodedPassword);
        _userService.Save(user);

        return user;
    }
}
