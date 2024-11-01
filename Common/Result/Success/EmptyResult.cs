using System.Diagnostics.CodeAnalysis;

namespace Zz;

public class EmptyResult : SuccessResult<Null>
{
    private EmptyResult()
        : base(IResult.StatusCode.Success_NoContent, Null.Value) { }

    private static readonly EmptyResult _emptySuccess = new();
    public static EmptyResult Value => _emptySuccess;

    private static readonly Task<EmptyResult> _emptySuccessAsync = Task.FromResult(_emptySuccess);
    public static Task<EmptyResult> ValueAsync => _emptySuccessAsync;
}

public static partial class Result
{
    public static EmptyResult Empty => EmptyResult.Value;

    public static Task<EmptyResult> EmptyAsync => EmptyResult.ValueAsync;

    public static bool IsEmptyResult(
        this IResult result,
        [NotNullWhen(true)] out EmptyResult? emptyResult
    )
    {
        if (result == Empty)
        {
            emptyResult = Empty;
            return true;
        }

        emptyResult = null;
        return false;
    }
}
