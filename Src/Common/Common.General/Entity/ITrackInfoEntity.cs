using System;

namespace Common.General.Entity
{
    public interface ITrackInfoEntity<T> : IEntity<T>
    {
        Guid CreatedBy { get; set; }
        DateTime CreationDate { get; set; }
    }
}
