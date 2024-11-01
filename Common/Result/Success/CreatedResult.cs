using System.Diagnostics.CodeAnalysis;

namespace Zz;

public class CreatedResult<T> : SuccessResult<T>
{
    public CreatedResult(T idOrValue)
        : base(IResult.StatusCode.Success_Created, idOrValue) { }
}

public static partial class Result
{
    public static CreatedResult<T> Created<T>(T idOrValue) => new CreatedResult<T>(idOrValue);

    public static Task<CreatedResult<T>> CreatedAsync<T>(T idOrValue) =>
        Task.FromResult(Created(idOrValue));

    public static bool IsCreatedResult<T>(
        this IResult result,
        [NotNullWhen(true)] out CreatedResult<T>? createdResult
    )
    {
        if (result is CreatedResult<T> res)
        {
            createdResult = res;
            return true;
        }

        createdResult = null;
        return false;
    }
}
