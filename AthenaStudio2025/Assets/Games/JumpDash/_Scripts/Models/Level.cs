using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JumpDash
{
    [Serializable]
    public class Level : ICloneable<Level>
    {
        [SerializeField] float beginDelayTime;
        [SerializeField] float deltaTime;

        public float BeginDelayTime { get => beginDelayTime; set => beginDelayTime = value; }
        public float DeltaTime { get => deltaTime; set => deltaTime = value; }
       
        public Level CloneSelf()
        {
            var serialized = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<Level>(serialized);
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}