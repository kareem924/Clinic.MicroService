using System;

namespace Common.General.Entity
{
    public class TrackInfoEntity<T> : BaseEntity<T>
    {
        public T CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
