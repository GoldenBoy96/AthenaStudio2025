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

    public enum WallType
    {
        Left,
        Right,
    }
}