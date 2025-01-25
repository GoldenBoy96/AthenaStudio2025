using UnityEngine;

namespace JumpDash
{
    public class WallController : MonoBehaviour
    {
        [SerializeField] WallType type;

        public WallType Type { get => type; }
    }
}