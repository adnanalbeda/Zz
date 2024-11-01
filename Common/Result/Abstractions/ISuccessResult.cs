namespace Zz;

public interface ISuccessResultContent<out T> : IResultContent
{
    public T Value { get; }
}

public interface ISuccessResult<out TData> : IResult
{
    public TData SuccessValue { get; }
}

public interface ISuccessResult<TData, TContent> : ISuccessResult<TData>, IResult<TContent>
    where TContent : ISuccessResultContent<TData>
{
    TData ISuccessResult<TData>.SuccessValue => this.Data.Value;
}
