using System;

namespace Common.General.Entity
{
    public class FullTrackInfoEntity<T> : TrackInfoEntity<T>
    {
        Guid UpdateBy { get; set; }

        DateTime UpdatingDate { get; set; }
    }

}
