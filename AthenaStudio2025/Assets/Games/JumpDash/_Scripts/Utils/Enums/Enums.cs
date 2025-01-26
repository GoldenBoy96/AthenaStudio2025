using UnityEngine;

namespace JumpDash
{
    public enum PlayerState
    {
        Idle,
        MoveLeft,
        MoveRight,
    }

    public enum ObstancleType
    {
        Mono,
        Wall
    }

    public enum LevelState
    {
        Pending,
        Playing,
        Ending
    }
    public enum ObstancleState
    {
        Idle,
        MoveDown
    }

    public enum WallType
    {
        Left,
        Right,
    }
}