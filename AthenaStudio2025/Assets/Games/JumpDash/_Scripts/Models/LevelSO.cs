using Newtonsoft.Json;
using UnityEngine;

namespace JumpDash
{
    [CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/JumpDash/Level", order = 1)]
    public class LevelSO : ScriptableObject
    {
        [SerializeField] Level level;

        public Level Level { get => level; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}