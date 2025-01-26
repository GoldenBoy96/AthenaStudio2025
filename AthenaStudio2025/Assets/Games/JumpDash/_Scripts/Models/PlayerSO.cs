using Newtonsoft.Json;
using UnityEngine;

namespace JumpDash
{
    [CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/JumpDash/Player", order = 1)]
    public class PlayerSO : ScriptableObject
    {
        [SerializeField] Player player;

        public Player Player { get => player;  }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}