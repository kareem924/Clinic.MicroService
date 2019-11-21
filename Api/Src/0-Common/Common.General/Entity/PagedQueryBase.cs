namespace Common.General.Entity
{
    public abstract class PagedQueryBase : IPagedQuery
    {
        public int Page { get; set; }
        public int Results { get; set; }
        public string OrderBy { get; set; }
        public SortOrder SortOrder { get; set; }
    }
}