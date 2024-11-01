namespace Zz;

public partial class AppException(string message, Exception? innerException = null)
    : Exception(message, innerException);
