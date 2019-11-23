using System.Collections.Generic;

namespace Common.General.Entity
{
    public interface IPagedFilter<TResult, in TQuery> where TQuery : PagedQueryBase
    {
        PagedResult<TResult> Filter(IEnumerable<TResult> values, PagedQueryBase query);
    }
}