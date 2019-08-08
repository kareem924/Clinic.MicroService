using System;

namespace Common.General.Entity
{
    public class TrackInfoEntity<T> : BaseEntity<T>
    {
        Guid CreatedBy { get; set; }
        DateTime CreationDate { get; set; }
    }
}
