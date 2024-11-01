namespace Zz;

public partial class AppArgumentException(
    string paramName,
    string? message = null,
    Exception? innerException = null
) : ArgumentException(message, paramName, innerException) { }
