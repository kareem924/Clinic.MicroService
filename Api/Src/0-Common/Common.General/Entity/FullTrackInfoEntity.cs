using System;

namespace Common.General.Entity
{
    public class FullTrackInfoEntity<T> : TrackInfoEntity<T>
    {
        T UpdateBy { get; set; }

        DateTime UpdatingDate { get; set; }
    }

}
