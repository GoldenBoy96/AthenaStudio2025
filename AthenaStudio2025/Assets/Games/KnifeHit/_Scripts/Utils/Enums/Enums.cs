using UnityEngine;

namespace KnifeHit
{
    public enum KnifeState
    {
        Pending,
        Flying,
        Attaching,
        Falling
    }

    public enum LogState
    {
        Rotating,
        Stopping,
        Falling
    }

    public enum LevelState
    {
        Playing,
        Winning,
        Losing
    }

    public enum ItemState
    {
        Pending,
        Falling
    }
}