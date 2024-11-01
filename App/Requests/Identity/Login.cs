namespace Zz.App.Requests;

using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zz.App.Core;
using Zz.Model.Identity;

public class Login
{
    public record HResult(
        Id22 Id,
        string? UserName,
        string? Email,
        string? DisplayName,
        string Token,
        string SessionKey
    );

    public record TwoFAResult(Id22 Id, string TwoFA_AccessToken);

    public class Request : ICommand<HResult>
    {
        public required string UsernameOrEmail { get; init; }
        public required string Password { get; init; }
        public bool RememberMe { get; init; }
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.UsernameOrEmail).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        }
    }

    public class Handler : IdentityRequestHandler<Request, HResult>
    {
        public Handler(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        protected override async Task<IRequestResult<HResult>> Execute(
            Request req,
            CancellationToken cancellationToken
        )
        {
            this.AppProcessLogger.LogInformation("Request :: Login :: Executing...");

            var query = UserManager.Users.Include(x => x.Profile);

            var user = await query.FirstOrDefaultAsync(
                x => x.UserName == req.UsernameOrEmail || x.Email == req.UsernameOrEmail,
                cancellationToken
            );

            if (cancellationToken.IsAborted())
                return await ABORTED();

            if (user is null)
            {
                this.AppProcessLogger.LogDebug("Request :: Login :: User not found.");

                return await UNPROCESSABLE(
                    AppErrorCodes.INVALID_DATA,
                    AppErrorCodes.INVALID_DATA,
                    [
                        new(
                            nameof(req.UsernameOrEmail),
                            [
                                new(
                                    "Account is not registered!",
                                    AppErrorCodes.Identity.ACCOUNT_NOT_REGISTERED
                                ),
                            ]
                        ),
                    ]
                );
            }

            if (await UserManager.IsLockedOutAsync(user))
            {
                this.AppProcessLogger.LogDebug("Request :: Login :: Too many retries.");

                return await Result
                    .TooManyRetriesAsyncResult(
                        user.LockoutEnd!.Value.UtcDateTime,
                        code: AppErrorCodes.Identity.LOCKED_OUT
                    )
                    .ToReqResAsync<HResult>();
            }

            var checkPasswordResult = await UserManager.CheckPasswordAsync(user, req.Password);

            if (!checkPasswordResult)
            {
                this.AppProcessLogger.LogDebug("Request :: Login :: Wrong Password.");

                await UserManager.AccessFailedAsync(user);

                return await UNPROCESSABLE(
                    AppErrorCodes.INVALID_DATA,
                    AppErrorCodes.INVALID_DATA,
                    [
                        new(
                            nameof(req.Password),
                            [new("Password is wrong!", AppErrorCodes.Identity.WRONG_PASSWORD)]
                        ),
                    ]
                );
            }

            if (cancellationToken.IsAborted())
                return await ABORTED();

            if (
                UserManager.SupportsUserTwoFactor
                && await UserManager.GetTwoFactorEnabledAsync(user)
            )
            {
                this.AppProcessLogger.LogDebug("Request :: Login :: 2FA Required.");

                return RequestResult<HResult>.FromResult(
                    await Result.AcceptedAsync(
                        user.Id.ToShortId(),
                        await TokenService.CreateScopedTokenAsync(
                            user.Id,
                            req.UsernameOrEmail,
                            AppScopedRolesDefaults.TWO_FA,
                            UtcNow.AddMinutes(20)
                        )
                    )
                );
            }
            if (cancellationToken.IsAborted())
                return await ABORTED();

            await UserManager.ResetAccessFailedCountAsync(user);

            var session = new UserSession(user.Id);

            await UserSessions.AddAsync(session);
            var res = await UserSessions.SaveChangesAsync();

            if (res < 1)
            {
                AppProcessLogger.LogWarning("Could not create session for {UID}.", user.Id);
                return await UNEXPECTED_END_OF_PROCESS();
            }

            DateTime exp = req.RememberMe
                ? DateTime.UtcNow.AddMonths(3)
                : DateTime.UtcNow.AddHours(12);

            var token = await TokenService.CreateTokenAsync(user, session.Id);

            // this.HttpContext.Response.Cookies.Append(
            //     "Authentication",
            //     string.Concat("Bearer ", token),
            //     new()
            //     {
            //         HttpOnly = true,
            //         Secure = true,
            //         MaxAge = exp - UtcNow,
            //     }
            // );

            return await SUCCESS_OK(
                new HResult(
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.Profile.DisplayName,
                    token,
                    session.GetEncryptedSessionId()
                )
            );
        }
    }
}
