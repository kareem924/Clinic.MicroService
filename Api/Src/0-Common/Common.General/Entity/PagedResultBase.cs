namespace Common.General.Entity
{
    public  class PagedResultBase
    {
        public int CurrentPage { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public long TotalItems { get; }

        protected PagedResultBase()
        {
        }

        protected PagedResultBase(int currentPage, int resultsPerPage,
            int totalPages, long totalItems)
        {
            CurrentPage = currentPage > totalPages ? totalPages : currentPage;
            PageSize = resultsPerPage;
            TotalPages = totalPages;
            TotalItems = totalItems;
        }
    }
}