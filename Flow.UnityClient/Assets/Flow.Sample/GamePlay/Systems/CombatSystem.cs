using System;
using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Systems.Base;
using UnityEngine;

namespace Flow.Sample.GamePlay.Systems
{
    public class CombatSystem : BaseUpdateEntitySystem
    {
        private readonly IEntityContainer _entityContainer;
        private readonly Collider[] _overlapBuffer = new Collider[100];
        
        protected override Type[] EntityFilter { get; }
        
        public CombatSystem(IEntityContainer entityContainer) : base(entityContainer)
        {
        }
        
        protected override void OnUpdateEntity(BaseEntity baseEntity, int index, float deltaTime)
        {
            throw new NotImplementedException();
        }

        public void DealDamage(BaseEntity attacker, BaseEntity target, float damage)
        {
            if (target == null || !target.IsValid) return;
            
            var targetHealth = target.GetComponent<HealthComponent>();
            if (targetHealth == null) return;
            
            float previousHealth = targetHealth.CurrentHealth;
            targetHealth.TakeDamage(damage);
        }
        
        public void DealAreaDamage(Vector3 position, float radius, float damage, DamageType damageType)
        {
            var count = Physics.OverlapSphereNonAlloc(position, radius, _overlapBuffer);
            for (int i = 0; i < count; ++i)
            {
                
            }

            foreach (var collider in _overlapBuffer)
            {
                var entity = collider.GetComponent<BaseEntity>();
                if (entity != null && entity.IsValid)
                {
                    var health = entity.GetComponent<HealthComponent>();
                    if (health != null)
                    {
                        health.TakeDamage(damage);
                        OnDamageDealt?.Invoke(entity, damage);
                    }
                }
            }
        }
        
        public void ApplyDebuff(BaseEntity target, DebuffType debuffType, float value, float duration)
        {
            if (target == null || !target.IsValid) return;
            
            switch (debuffType)
            {
                case DebuffType.Slow:
                    var enemyEntity = target as EnemyEntity;
                    enemyEntity?.ApplySlow(value, duration);
                    break;
                    
                case DebuffType.Burn:
                    // Apply damage over time
                    break;
                    
                case DebuffType.Freeze:
                    // Stop movement completely
                    var speed = target.GetComponent<SpeedComponent>();
                    if (speed != null)
                    {
                        speed.SpeedMultiplier = 0f;
                    }
                    break;
            }
        }
    }
    
    public enum DebuffType
    {
        None,
        Slow,
        Burn,
        Freeze,
        Poison,
        Stun
    }
}
