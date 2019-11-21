namespace Common.General.Entity
{
    public interface IPagedQuery : IQuery
    {
        int Page { get; }
        int Results { get; }
        string OrderBy { get; }
        SortOrder SortOrder { get; }
    }
}