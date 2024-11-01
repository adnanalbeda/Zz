using System.Diagnostics.CodeAnalysis;

namespace Zz;

public class OkResult<T> : SuccessResult<T>
{
    public OkResult(T value)
        : base(IResult.StatusCode.Success_Ok, value) { }
}

public static partial class Result
{
    public static OkResult<T> Ok<T>(T value) => new OkResult<T>(value);

    public static Task<OkResult<T>> OkAsync<T>(T value) => Task.FromResult(Ok(value));

    public static Task<ISuccessResult<T>> OkAsyncResult<T>(T value) =>
        Task.FromResult((ISuccessResult<T>)Ok(value));

    public static bool IsOkResult<T>(
        this IResult result,
        [NotNullWhen(true)] out OkResult<T>? okResult
    )
    {
        if (result is OkResult<T> res)
        {
            okResult = res;
            return true;
        }

        okResult = null;
        return false;
    }
}
