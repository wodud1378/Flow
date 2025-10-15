using System;
using UnityEngine;

namespace Flow.Sample.GamePlay.Components
{
    public class HealthComponent : MonoBehaviour, IComponent
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth;
        
        public float MaxHealth => maxHealth;
        public float CurrentHealth => currentHealth;
        public float HealthPercentage => currentHealth / maxHealth;
        public bool IsAlive => currentHealth > 0;
        
        public event Action<float> OnHealthChanged;
        public event Action OnDeath;
        
        private void Awake()
        {
            currentHealth = maxHealth;
        }
        
        public void TakeDamage(float damage)
        {
            if (!IsAlive) return;
            
            currentHealth = Mathf.Max(0, currentHealth - damage);
            OnHealthChanged?.Invoke(currentHealth);
            
            if (currentHealth <= 0)
            {
                OnDeath?.Invoke();
            }
        }
        
        public void Heal(float amount)
        {
            if (!IsAlive) return;
            
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            OnHealthChanged?.Invoke(currentHealth);
        }
        
        public void SetMaxHealth(float newMaxHealth)
        {
            maxHealth = newMaxHealth;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }
    }
}
