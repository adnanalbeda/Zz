using System.Diagnostics.CodeAnalysis;

namespace Zz;

public class AcceptedResult : SuccessResult<AcceptedResult.Data>
{
    public AcceptedResult(string id, string? url)
        : base(IResult.StatusCode.Success_Accepted, new(id, url)) { }

    public record struct Data(string Id, string? Url);
}

public static partial class Result
{
    public static AcceptedResult Accepted(string id, string? url) => new AcceptedResult(id, url);

    public static Task<AcceptedResult> AcceptedAsync(string trackId, string? trackUrl) =>
        Task.FromResult(Accepted(trackId, trackUrl));

    public static Task<ISuccessResult<AcceptedResult.Data>> AcceptedAsyncResult<TRes>(
        string trackId,
        string? trackUrl
    ) => Task.FromResult<ISuccessResult<AcceptedResult.Data>>(Accepted(trackId, trackUrl));

    public static bool IsAcceptedResult(
        this IResult result,
        [NotNullWhen(true)] out AcceptedResult? acceptedResult
    )
    {
        if (result is AcceptedResult res)
        {
            acceptedResult = res;
            return true;
        }

        acceptedResult = null;
        return false;
    }
}
