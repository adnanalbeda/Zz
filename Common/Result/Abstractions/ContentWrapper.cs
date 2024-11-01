namespace Zz;

public class ContentWrapper(IResultContent data)
{
    public IResultContent Data { get; } = data;
}

public class ContentWrapper<T>(T data) : ContentWrapper(data)
    where T : IResultContent
{
    public new T Data => data;
}
