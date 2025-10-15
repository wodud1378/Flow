using System;
using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Systems.Base;

namespace Flow.Sample.GamePlay.Systems
{
    public class EnemySystem : BaseUpdateEntitySystem
    {
        protected override Type[] EntityFilter => new[] { typeof(EnemyEntity) };
        
        public EnemySystem(IEntityContainer entityContainer) : base(entityContainer, 64)
        {
        }
        
        protected override void OnUpdateEntity(BaseEntity baseEntity, int index, float deltaTime)
        {
            var enemy = baseEntity as EnemyEntity;
            if (enemy == null) return;
            
            var enemyComponent = enemy.GetComponent<EnemyComponent>();
            var healthComponent = enemy.GetComponent<HealthComponent>();
            
            if (enemyComponent == null) return;
            
            // Move enemy along path
            enemy.MoveAlongPath(deltaTime);
            
            // Check if enemy is dead
            if (healthComponent != null && !healthComponent.IsAlive)
            {
                // Death handling is done in EnemyEntity
                return;
            }
            
            // Check if enemy reached end
            if (enemyComponent.HasReachedEnd)
            {
                // End handling is done in EnemyEntity
                return;
            }
        }
    }
}
