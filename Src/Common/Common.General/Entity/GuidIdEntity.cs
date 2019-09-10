using System;
using System.Collections.Generic;
using System.Text;

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
