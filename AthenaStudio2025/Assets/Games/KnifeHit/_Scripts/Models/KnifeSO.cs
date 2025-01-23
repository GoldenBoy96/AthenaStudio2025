using Newtonsoft.Json;
using UnityEngine;

namespace KnifeHit
{
    [CreateAssetMenu(fileName = "Knife", menuName = "ScriptableObjects/Knife", order = 1)]
    public class KnifeSO : ScriptableObject
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private Knife knife;

        public Knife Knife { get => knife; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}