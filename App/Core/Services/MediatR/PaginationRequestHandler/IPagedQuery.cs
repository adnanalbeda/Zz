using Zz.Pagination;

namespace Zz.App.Core;

public interface IPagedQuery<T> : IPagingParams, IQuery<IPagination<T>> { }
