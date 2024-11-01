namespace Zz.App.Core;

using System;
using Microsoft.AspNetCore.Identity;
using Zz.App.Identity;
using Zz.DataBase.Identity;
using Zz.Model.Identity;

public abstract partial class IdentityRequestHandler<TReq, TRes>(IServiceProvider serviceProvider)
    : RequestHandler<TReq, TRes>(serviceProvider)
    where TReq : IAppRequest<TRes>
{
    private SignInManager<User>? _SignInManager;
    protected SignInManager<User> SignInManager =>
        _SignInManager ??= GetRequiredService<SignInManager<User>>();

    private UserManager<User>? _UserManager;
    protected UserManager<User> UserManager =>
        _UserManager ??= GetRequiredService<UserManager<User>>();

    private RoleManager<Role>? _RoleManager;
    protected RoleManager<Role> RoleManager =>
        _RoleManager ??= GetRequiredService<RoleManager<Role>>();

    private ITokenService? _TokenService;
    protected ITokenService TokenService => _TokenService ??= GetRequiredService<ITokenService>();

    private IChallenge? _Challenge;
    protected IChallenge Challenge => _Challenge ??= GetRequiredService<IChallenge>();

    private UserSessionsDataContext? _UserSessionsDataContext;
    protected UserSessionsDataContext UserSessions =>
        _UserSessionsDataContext ??= GetRequiredService<UserSessionsDataContext>();
}
