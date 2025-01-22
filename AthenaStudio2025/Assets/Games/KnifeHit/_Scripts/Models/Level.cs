using Newtonsoft.Json;
using System;
using UnityEngine;
namespace KnifeHit
{
    [Serializable]
    public class Level : ICloneable<Level>
    {
        [SerializeField] private LogSO log;
        [SerializeField] private KnifeSO knife;
        [SerializeField] private int knifeAmount;


        public Level CloneSelf()
        {
            var serialized = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<Level>(serialized);
        }
    }
}
