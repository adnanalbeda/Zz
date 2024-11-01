namespace Zz;

public interface IResultContent;

public partial interface IResult
{
    public StatusCode Status { get; }

    public ContentWrapper Content { get; }
}

public interface IResult<T> : IResult
    where T : IResultContent
{
    public new ContentWrapper<T> Content { get; }

    ContentWrapper IResult.Content => this.Content;

    T Data => this.Content.Data;
}
