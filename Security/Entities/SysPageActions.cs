using System;

namespace Abstract.Security.Entities
{
    public partial class SysPageActions
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public int ActionId { get; set; }
        public bool IsAllowed { get; set; }
        public int? RoleId { get; set; }

       
    }
}
