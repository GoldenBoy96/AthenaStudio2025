using UnityEngine;

namespace KnifeHit
{
    [CreateAssetMenu(fileName = "Log", menuName = "ScriptableObjects/Log", order = 2)]
    public class LogSO : ScriptableObject
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private Log log;

        public Log Log { get => log; set => log = value; }
    }
}