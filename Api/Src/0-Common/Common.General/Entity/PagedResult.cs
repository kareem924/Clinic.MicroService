using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Common.General.Entity
{
    public class PagedResult<T> : PagedResultBase
    {
        public IEnumerable<T> Items { get; }

        public bool IsEmpty => Items == null || !Items.Any();
        public bool IsNotEmpty => !IsEmpty;

        protected PagedResult()
        {
            Items = Enumerable.Empty<T>();
        }

        [JsonConstructor]
        protected PagedResult(
            IEnumerable<T> items,
            int currentPage, 
            int resultsPerPage,
            int totalPages, 
            long totalItems) :
                base(currentPage, resultsPerPage, totalPages, totalItems)
        {
            Items = items;
        }

        public static PagedResult<T> Create(IEnumerable<T> items,
            int currentPage, int resultsPerPage,
            int totalPages, long totalResults)
            => new PagedResult<T>(items, currentPage, resultsPerPage, totalPages, totalResults);

        public static PagedResult<T> From(PagedResultBase result, IEnumerable<T> items)
            => new PagedResult<T>(items, result.CurrentPage, result.PageSize,
                result.TotalPages, result.TotalItems);

        public static PagedResult<T> Empty => new PagedResult<T>();
    }
}