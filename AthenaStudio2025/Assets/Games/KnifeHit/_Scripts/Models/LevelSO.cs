using Newtonsoft.Json;
using UnityEngine;

namespace KnifeHit
{
    [CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
    public class LevelSO : ScriptableObject
    {
        [SerializeField] private Level level;

        public Level Level { get => level; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
