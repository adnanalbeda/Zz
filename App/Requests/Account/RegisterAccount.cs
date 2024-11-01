namespace Zz.App.Requests;

using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Zz.App.Core;
using Zz.DataBase;
using Zz.Model.Identity;

public class RegisterAccount
{
    public record HResult(Id22 Id, string? UserName, string Token);

    public class Request : ICommand<HResult>
    {
        public required string UserName { get; init; }
        public required string Password { get; init; }
        public required string RepeatPassword { get; init; }

        public required string ChallengeTokenData { get; set; }
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator(DataContext dataContext, IChallenge challenge)
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .Matches(@"^[a-zA-Z0-9_]+$")
                .MustAsync((x, c) => dataContext.Users.AnyAsync(y => y.UserName == x, c))
                .WithMessage("Username is already in use.")
                .WithErrorCode(AppErrorCodes.Identity.USERNAME_USED);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches(@"[0-9]+")
                .WithErrorCode(PasswordFormatErrorCode)
                .WithMessage(PasswordFormatErrorMessage)
                .Matches(@"[a-z]+")
                .WithErrorCode(PasswordFormatErrorCode)
                .WithMessage(PasswordFormatErrorMessage)
                .Matches(@"[A-Z]+")
                .WithErrorCode(PasswordFormatErrorCode)
                .WithMessage(PasswordFormatErrorMessage)
                .Matches(@"\W+")
                .WithErrorCode(PasswordFormatErrorCode)
                .WithMessage(PasswordFormatErrorMessage);
            RuleFor(x => x.RepeatPassword)
                .Must((x, y) => x.Password == y)
                .WithErrorCode(AppErrorCodes.Identity.PASSWORDS_NEW_REPEAT_NOT_EQUAL);

            RuleFor(x => x.ChallengeTokenData)
                .Must(x => challenge.ValidateChallengeTokenData(x))
                .WithMessage("Challenge failed. Please, try again!");
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
            this.AppProcessLogger.LogDebug("Register Account...");

            var user = new User(Id22.New(), req.UserName)
            {
                MetaData = new(default),
                Profile = new(default),
                Settings = new(default),
            };

            var res = await UserManager.CreateAsync(user, req.Password);

            if (res.Succeeded)
                return await SUCCESS_OK(
                    new(user.Id, user.UserName, await TokenService.CreateTokenAsync(user))
                );

            return await UNEXPECTED_END_OF_PROCESS();
        }
    }
}
