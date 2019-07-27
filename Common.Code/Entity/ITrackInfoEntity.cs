using System;

namespace Common.Code.Entity
{
    public interface ITrackInfoEntity<T> : IEntity<T>
    {
        Guid CreatedBy { get; set; }
        DateTime CreationDate { get; set; }
    }
}
