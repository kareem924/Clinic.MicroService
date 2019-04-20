using System;

namespace Abstract.Security.Entities
{
    public partial class SysPages
    {
     
        public int Id { get; set; }
        public string PageName { get; set; }
        public int ParentId { get; set; }

       
    }
}
