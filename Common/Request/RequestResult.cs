namespace Zz;

public interface IRequestResult
{
    IResult Result { get; }
}

public interface IRequestResult<out T> : IRequestResult
{
    public T? SuccessValue() =>
        this.Result.Status.IsSuccess() && this.Result is ISuccessResult<T> success
            ? success.SuccessValue
            : default;
}

public struct RequestResult<T> : IRequestResult<T>
{
    public IResult Result { get; }

    private RequestResult(IResult result)
    {
        this.Result = result;
    }

    public static RequestResult<T> FromResult(IResult result) => new(result);

    public static RequestResult<T> FromSuccess<TS>(TS result)
        where TS : ISuccessResult<T> => new(result);

    public static RequestResult<T> FromError<TE>(TE result)
        where TE : IErrorResult => new(result);
}

public static class RequestResultExtensions
{
    public static IRequestResult<T> ToReqRes<T>(this ISuccessResult<T> result) =>
        RequestResult<T>.FromSuccess(result);

    public static IRequestResult<T> ToReqRes<T>(this IErrorResult result) =>
        RequestResult<T>.FromError(result);

    public static IRequestResult<T> ToReqRes<T, TR>(this TR result)
        where TR : IResult => RequestResult<T>.FromResult(result);

    public static Task<IRequestResult<T>> ToReqResAsync<T>(this ISuccessResult<T> result) =>
        Task.FromResult(ToReqRes(result));

    public static Task<IRequestResult<T>> ToReqResAsync<T>(this IErrorResult result) =>
        Task.FromResult(ToReqRes<T>(result));

    public static Task<IRequestResult<T>> ToReqResAsync<T, TR>(this TR result)
        where TR : IResult => Task.FromResult(ToReqRes<T, TR>(result));

    public static async Task<IRequestResult<T>> ToReqResAsync<T>(
        this Task<ISuccessResult<T>> result
    ) => await ToReqResAsync(await result);

    public static async Task<IRequestResult<T>> ToReqResAsync<T>(this Task<IErrorResult> result) =>
        await ToReqResAsync<T>(await result);

    public static async Task<IRequestResult<T>> ToReqResAsync<T, TR>(this Task<TR> result)
        where TR : IResult => await ToReqResAsync<T, TR>(await result);
}
