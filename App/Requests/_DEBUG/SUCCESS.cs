namespace Zz.App.Requests;

using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Zz.App.Core;

public class SuccessRequest
{
    public record HResult(bool Success);

    public class Request : IQuery<HResult>
    {
        public required string CurrentPassword { get; init; }
        public required string NewPassword { get; init; }
        public required string RepeatNewPassword { get; init; }
    }

    public class Handler : IdentityRequestHandler<Request, HResult>
    {
        public Handler(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        protected override Task<IRequestResult<HResult>> Execute(
            Request req,
            CancellationToken cancellationToken
        )
        {
            return SUCCESS_OK(new(true));
        }
    }
}
