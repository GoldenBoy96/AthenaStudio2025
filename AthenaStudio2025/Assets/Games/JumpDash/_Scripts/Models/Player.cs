using Newtonsoft.Json;
using System;
using UnityEngine;

namespace JumpDash
{
    [Serializable]
    public class Player : ICloneable<Player>
    {

        [SerializeField] float speed;

        public float Speed { get => speed; }

        public Player CloneSelf()
        {
            var serialized = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<Player>(serialized);
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}