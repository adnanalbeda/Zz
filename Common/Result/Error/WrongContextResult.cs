using System.Diagnostics.CodeAnalysis;

namespace Zz;

public record struct WrongContextResultContent(string Message, string Code) : IErrorResultContent;

/// <summary>
/// How is this client fault? They shouldn't request data that don't exist.
/// </summary>
public class WrongContextResult(string message, string code)
    : ErrorResult<WrongContextResultContent>(
        IResult.StatusCode.Error_WrongContext,
        new(message, code)
    );

public static partial class Result
{
    public const string IAM_A_TEAPOT_STRING_MESSAGE =
        "I cannot fill your cup with coffee. I am a teapot.";
    public const string IAM_A_TEAPOT_STRING_CODE = "TEAPOT";

    public static WrongContextResult WrongContext(
        string message = IAM_A_TEAPOT_STRING_MESSAGE,
        string code = IAM_A_TEAPOT_STRING_CODE
    ) => new(message, code);

    public static Task<WrongContextResult> WrongContextAsync(
        string message = IAM_A_TEAPOT_STRING_MESSAGE,
        string code = IAM_A_TEAPOT_STRING_CODE
    ) => Task.FromResult(WrongContext(message, code));

    public static Task<IErrorResult> WrongContextAsyncResult(
        string message = IAM_A_TEAPOT_STRING_MESSAGE,
        string code = IAM_A_TEAPOT_STRING_CODE
    ) => Task.FromResult<IErrorResult>(WrongContext(message, code));

    public static bool IsWrongContextResult(
        this IResult result,
        [NotNullWhen(true)] out WrongContextResult? notFoundResult
    )
    {
        if (result is WrongContextResult res)
        {
            notFoundResult = res;
            return true;
        }

        notFoundResult = null;
        return false;
    }
}
