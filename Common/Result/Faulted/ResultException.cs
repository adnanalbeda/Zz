namespace Zz;

public class ResultException : Exception, IErrorResultContent
{
    public ResultException(
        string message,
        Exception? innerException = null,
        ReqInfo? reqInfo = null
    )
        : base(message, innerException)
    {
        this.RequestInfo = reqInfo;
    }

    public ReqInfo? RequestInfo { get; }
}
