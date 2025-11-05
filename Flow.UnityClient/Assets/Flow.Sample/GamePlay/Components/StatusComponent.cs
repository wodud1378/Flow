using Flow.Sample.Entities;
using Flow.Sample.GamePlay.Components.Interfaces;
using UnityEngine;

namespace Flow.Sample.GamePlay.Components
{
    public class StatusComponent : MonoBehaviour, IComponent
    {
        [field:SerializeField] public BaseEntity Owner { get; private set; }
        
        [field:SerializeField] public float MaxHp { get; set; }
        [field:SerializeField] public float Damage { get; set; }
        [field:SerializeField] public float CriticalRate { get; set; }
        [field:SerializeField] public float CriticalMultiplier { get; set; }
        [field:SerializeField] public float RemainHp { get; set; }

        private void OnValidate()
        {
            if (Owner == null)
                Owner = GetComponent<BaseEntity>();

            RemainHp = MaxHp;
        }
    }
}