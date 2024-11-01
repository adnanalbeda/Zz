namespace Zz.App.Requests;

using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Zz.App.Core;
using Zz.Model.Identity;

public class SignInByEmail
{
    public record HResult(Id22 Id, string? UserName, string? Email, string? DisplayName);

    public class Request : ICommand<HResult>
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
        public bool RememberMe { get; init; }
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
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
            this.AppProcessLogger.LogDebug("Sign user in...");

            var user = await UserManager.FindByEmailAsync(req.Email);

            if (user is null)
            {
                return await UNPROCESSABLE("Email is not registered!", "EMAIL_NOT_USED");
            }

            var signInResult = await SignInManager.PasswordSignInAsync(
                user,
                req.Password,
                req.RememberMe,
                true
            );

            if (signInResult.Succeeded)
            {
                return await SUCCESS_OK(
                    new HResult(user.Id, user.UserName, user.Email, user.Profile.DisplayName)
                );
            }

            if (signInResult.IsLockedOut)
            {
                return await Result
                    .TooManyRequestsAsyncResult(
                        user.LockoutEnd?.UtcDateTime,
                        code: "LOCKED_FOR_TOO_MANY_RETRIES"
                    )
                    .ToReqResAsync<HResult>();
            }

            if (signInResult.RequiresTwoFactor)
            {
                return await TWO_FA(user);
            }

            if (!signInResult.IsNotAllowed)
            {
                return await UNPROCESSABLE("Wrong Credentials!", "INCORRECT_PASSWORD");
            }

            return await UNEXPECTED_END_OF_PROCESS();
        }

        private async Task<IRequestResult<HResult>> TWO_FA(User user)
        {
            this.AppProcessLogger.LogDebug(
                "How did we get here? 2FA is not supported yet. Especially for ({UID}).",
                user.Id
            );

            return await Result
                .NotImplementedAsyncResult("2FA is not currently supported!")
                .ToReqResAsync<HResult>();

            // FOR FUTURE
            // return RequestResult<HResult>.FromResult<HResult>(
            //     await Result.AcceptedAsync("", "")
            // );
        }
    }
}
