using System;
using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;
using Flow.Sample.GamePlay.Systems.Base;

namespace Flow.Sample.GamePlay.Systems
{
    public class CombatSystem : BaseUpdateEntitySystem
    {
        protected override Type[] EntityFilter { get; }
        
        public CombatSystem(IEntityContainer entityContainer) : base(entityContainer)
        {
        }

        protected override void OnUpdateEntity(BaseEntity baseEntity, int index, float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}