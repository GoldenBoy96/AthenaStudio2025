using Newtonsoft.Json;
using UnityEngine;

namespace KnifeHit
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/KnifeHit/Item", order = 1)]
    public class ItemSO : ScriptableObject
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private Item item;

        public Item Item { get => item; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}