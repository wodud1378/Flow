using Flow.Sample.Entities;
using Flow.Sample.GamePlay.Components.Interfaces;
using UnityEngine;

namespace Flow.Sample.GamePlay.Components
{
    public class HealthComponent : MonoBehaviour, IComponent
    {
        [field:SerializeField] private float health;
        
        public BaseEntity Owner { get; private set; }
        public float Health { get; private set; }
        
        public void Initialize(BaseEntity owner)
        {
            Owner = owner;
            Health = health;
        }
        
        public void Increase(float value)
        {
            Health += value;
        }
        
        public void Decrease(float value)
        {
            Health = Mathf.Max(0, Health - value);
        }
    }
}