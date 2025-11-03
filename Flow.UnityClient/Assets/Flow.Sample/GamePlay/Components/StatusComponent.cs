using Flow.Sample.Entities;
using Flow.Sample.GamePlay.Components.Interfaces;
using UnityEngine;

namespace Flow.Sample.GamePlay.Components
{
    public class StatusComponent : MonoBehaviour, IComponent
    {
        [field:SerializeField] public float MaxHp { get; private set; }
        [field:SerializeField] public float Damage { get; private set; }
        [field:SerializeField] public float CriticalRate { get; private set; }
        [field:SerializeField] public float CriticalMultiplier { get; private set; }
        
        public BaseEntity Owner { get; private set; }
        
        public float RemainHp { get; private set; }

        private void Awake()
        {
            Owner = GetComponent<BaseEntity>();
            RemainHp = MaxHp;
        }
    }
}