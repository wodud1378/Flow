using System;
using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;
using Flow.Sample.GamePlay.Systems.Base;

namespace Flow.Sample.GamePlay.Systems
{
    public class ProjectileSystem : BaseUpdateEntitySystem
    {
        protected override Type[] EntityFilter => new[] { typeof(ProjectileEntity) };
        
        public ProjectileSystem(IEntityContainer entityContainer) : base(entityContainer, 256)
        {
        }
        
        protected override void OnUpdateEntity(BaseEntity baseEntity, int index, float deltaTime)
        {
            var projectile = baseEntity as ProjectileEntity;
            if (projectile == null) 
                return;
            
            // Update projectile movement
            projectile.UpdateMovement(deltaTime);
        }
    }
}
