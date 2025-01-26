using Newtonsoft.Json;
using UnityEngine;

namespace KnifeHit
{
    [CreateAssetMenu(fileName = "Log", menuName = "ScriptableObjects/KnifeHit/Log", order = 2)]
    public class LogSO : ScriptableObject
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private Log log;

        public Log Log { get => log; set => log = value; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}