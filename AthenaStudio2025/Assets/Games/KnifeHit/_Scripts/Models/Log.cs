using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KnifeHit
{
    [Serializable]
    public class Log : ICloneable<Log>
    {
        [SerializeField] List<LogRotationElement> rotationPattern;

        public List<LogRotationElement> RotationPattern { get => rotationPattern; }

        public Log CloneSelf()
        {
            var serialized = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<Log>(serialized);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}