namespace Zz.App.Requests;

using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Zz.App.Core;

public class ChangePassword
{
    public record HResult(bool Success);

    public class Request : ICommand<HResult>
    {
        public required string CurrentPassword { get; init; }
        public required string NewPassword { get; init; }
        public required string RepeatNewPassword { get; init; }
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().MinimumLength(6);

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(8)
                .WithMessage(PasswordFormatErrorMessage)
                .WithErrorCode(PasswordFormatErrorCode)
                .Matches(".*[a-z]")
                .WithMessage(PasswordFormatErrorMessage)
                .WithErrorCode(PasswordFormatErrorCode)
                .Matches(".*[A-Z]")
                .WithMessage(PasswordFormatErrorMessage)
                .WithErrorCode(PasswordFormatErrorCode)
                .Matches(".*[0-9]")
                .WithMessage(PasswordFormatErrorMessage)
                .WithErrorCode(PasswordFormatErrorCode)
                .Matches(@".*\W")
                .WithMessage(PasswordFormatErrorMessage)
                .WithErrorCode(PasswordFormatErrorCode);

            RuleFor(x => x.RepeatNewPassword)
                .Must((x, y) => x.NewPassword == y)
                .WithMessage("Passwords don't match!")
                .WithErrorCode(AppErrorCodes.Identity.PASSWORDS_NEW_REPEAT_NOT_EQUAL);
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
            this.AppProcessLogger.LogDebug("Getting Signed In User Info...");

            if (!User.IsSignedIn())
            {
                return await WRONG_CONTEXT();
            }

            var user = await UserManager.FindByIdAsync(User.Id.Value.ToString());

            if (user is null)
            {
                this.AppProcessLogger.LogCritical(
                    "User (({U_ID}):({U_NAME}):({U_EMAIL})) has reached access without authorization.",
                    User.Id,
                    User.UserName,
                    User.Email
                );
                return await NOT_FOUND(
                    "Your account is not found. Are you logged in?",
                    AppErrorCodes.Identity.ACCOUNT_NOT_FOUND
                );
            }

            var updatePasswordResult = await UserManager.ChangePasswordAsync(
                user,
                req.CurrentPassword,
                req.NewPassword
            );

            if (updatePasswordResult.Succeeded)
            {
                return await SUCCESS_OK(new(true));
            }

            if (updatePasswordResult.Errors.Any())
            {
                bool wrongPassword = false;
                bool complex = false;
                foreach (var errorCode in updatePasswordResult.Errors.Select(e => e.Code))
                {
                    if (errorCode == nameof(IdentityErrorDescriber.PasswordMismatch))
                    {
                        wrongPassword = true;
                    }
                    if (errorCode.Contains("PasswordRequires"))
                    {
                        complex = true;
                    }
                }
                return await UNPROCESSABLE(
                    AppErrorCodes.INVALID_DATA,
                    AppErrorCodes.INVALID_DATA,
                    NewEnumerable<InvalidRequestResultContent.Error>()
                        .ConcatToIf(
                            new ConditionalValue<InvalidRequestResultContent.Error>(
                                new(
                                    nameof(Request.CurrentPassword),
                                    [
                                        new(
                                            "Password is wrong.",
                                            AppErrorCodes.Identity.WRONG_PASSWORD
                                        ),
                                    ]
                                ),
                                wrongPassword
                            ),
                            new ConditionalValue<InvalidRequestResultContent.Error>(
                                new(
                                    nameof(Request.NewPassword),
                                    [new(PasswordFormatErrorMessage, PasswordFormatErrorCode)]
                                ),
                                complex
                            )
                        )
                );
            }

            return await UNEXPECTED_END_OF_PROCESS();
        }
    }
}
