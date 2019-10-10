using System;

namespace Common.General.Entity
{
   public class GuidIdEntity : BaseEntity<Guid>
    {
        public GuidIdEntity()
        {
            Id  = Guid.NewGuid();
        }
    }
}
