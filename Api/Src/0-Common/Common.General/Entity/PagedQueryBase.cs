namespace Common.General.Entity
{
    public abstract class PagedQueryBase
    {

        public int Page { get; set; }
        public int PageSize { get; set; }

        public int Skip => (Page - 1) * PageSize;
    }
}