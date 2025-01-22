using Newtonsoft.Json;
using System;
using UnityEngine;


namespace KnifeHit
{
    [Serializable]
    public class Knife : ICloneable<Knife>
    {
        [SerializeField] private float flyingSpeed = 1.0f;

        public Knife(float flyingSpeed)
        {
            this.flyingSpeed = flyingSpeed;
        }

        public float FlyingSpeed { get => flyingSpeed; set => flyingSpeed = value; }

        public Knife CloneSelf()
        {
            var serialized = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<Knife>(serialized);
        }
    }
}