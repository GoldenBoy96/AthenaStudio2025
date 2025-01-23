using Newtonsoft.Json;
using System;
using UnityEngine;
namespace KnifeHit
{
    [Serializable]
    public class Level : ICloneable<Level>
    {
        [SerializeField] LogController logPrefab;
        [SerializeField] KnifeController knifePrefab;
        [SerializeField] private int knifeAmount;
        [SerializeField] private float reloadCooldown;

        public LogController LogPrefab { get => logPrefab; }
        public KnifeController KnifePrefab { get => knifePrefab; }
        public int KnifeAmount { get => knifeAmount; }
        public float ReloadCooldown { get => reloadCooldown; }

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
