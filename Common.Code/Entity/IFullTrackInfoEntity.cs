using System;

namespace Common.Code.Entity
{
    public interface IFullTrackInfoEntity<T> : ITrackInfoEntity<T>
    {
        Guid UpdateBy { get; set; }

        DateTime UpdatingDate { get; set; }
    }
}
