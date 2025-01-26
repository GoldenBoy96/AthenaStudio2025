using KnifeHit;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace JumpDash
{
    [Serializable]
    public class Obstancle : ICloneable<Obstancle>
    {
        [SerializeField] float speed;
        [SerializeField] float randomPositionRange;

        public float Speed { get => speed; set => speed = value; }
        public float RandomPositionRange { get => randomPositionRange; set => randomPositionRange = value; }

        public Obstancle CloneSelf()
        {
            var serialized = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<Obstancle>(serialized);
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}