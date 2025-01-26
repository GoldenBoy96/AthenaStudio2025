using Newtonsoft.Json;
using UnityEngine;

namespace JumpDash
{
    [CreateAssetMenu(fileName = "Obstancle", menuName = "ScriptableObjects/JumpDash/Obstancle", order = 1)]
    public class ObstancleSO : ScriptableObject
    {
        [SerializeField] Obstancle obstancle;

        public Obstancle Obstancle { get => obstancle; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}