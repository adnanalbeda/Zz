using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Zz;

public class InvalidCurrencyFormatArgumentException(
    string value,
    string paramName,
    string? message = null,
    Exception? innerException = null
)
    : AppArgumentException(
        paramName,
        message ?? CurrencyFormatErrorMessage,
        innerException ?? new Exception(CurrencyFormatDetailedErrorMessage)
    )
{
    public string? Value { get; } = value;
}

public partial class AppArgumentException
{
    public static void ThrowIfInvalidCurrencyFormat(
        [NotNull] string? argument,
        [CallerArgumentExpression("argument")] string paramName = ""
    )
    {
        ThrowIfNullOrWhiteSpace(argument, paramName);

        if (!CurrencyRegex.IsMatch(argument))
            throw new InvalidCurrencyFormatArgumentException(argument, paramName);
    }
}
