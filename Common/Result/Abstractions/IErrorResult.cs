namespace Zz;

public interface IErrorResultContent : IResultContent;

public interface IErrorResult : IResult;

public interface IErrorResult<T> : IErrorResult, IResult<T>
    where T : IErrorResultContent;
