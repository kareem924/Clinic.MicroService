using System;

namespace Common.General.Entity
{
    public interface IFullTrackInfoEntity<T> : ITrackInfoEntity<T>
    {
        Guid UpdateBy { get; set; }

        DateTime UpdatingDate { get; set; }
    }
}
