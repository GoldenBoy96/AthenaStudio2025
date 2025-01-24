
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace KnifeHit
{
    [Serializable]
    public class Item : ICloneable<Item>
    {
        [SerializeField] float distance;
        [SerializeField] float degree;
        public Item(float distance = 0, float degree = 0)
        {
            this.distance = distance;
            this.degree = degree;
        }

        public float Distance { get => distance;}
        public float Degree { get => degree;}

        public Item CloneSelf()
        {
            var serialized = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<Item>(serialized);
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}