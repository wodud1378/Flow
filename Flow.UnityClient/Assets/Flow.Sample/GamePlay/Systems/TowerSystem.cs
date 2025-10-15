using System;
using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Systems.Base;

namespace Flow.Sample.GamePlay.Systems
{
    public class TowerSystem : BaseUpdateEntitySystem
    {
        protected override Type[] EntityFilter => new[] { typeof(TowerEntity) };
        
        public TowerSystem(IEntityContainer entityContainer) : base(entityContainer, 64)
        {
        }
        
        protected override void OnUpdateEntity(BaseEntity baseEntity, int index, float deltaTime)
        {
            var tower = baseEntity as TowerEntity;
            if (tower == null) return;
            
            var towerComponent = tower.GetComponent<TowerComponent>();
            var targetingComponent = tower.GetComponent<TargetingComponent>();
            
            if (towerComponent == null || targetingComponent == null) return;
            
            // Update targeting
            targetingComponent.SelectTarget();
            
            // Attack if has target and can attack
            if (targetingComponent.HasTarget && towerComponent.CanAttack())
            {
                tower.Attack(targetingComponent.CurrentTarget);
            }
        }
    }
}
