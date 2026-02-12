using System;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Entities;
using Flow.Sample.GamePlay.Entities.Interfaces;
using Flow.Sample.GamePlay.Systems.Base;
using VContainer;

namespace Flow.Sample.GamePlay.Systems
{
    public class StatusEffectSystem : BaseUpdateEntitySystem
    {
        protected override Type[] EntityFilter { get; } = { typeof(StatusEffectComponent) };

        [Inject]
        public StatusEffectSystem(IEntityContainer entityContainer)
            : base(entityContainer, 256)
        {
        }

        protected override void OnUpdateEntity(BaseEntity entity, int index, float deltaTime)
        {
            var effectComponent = As<StatusEffectComponent>(entity);
            effectComponent.UpdateEffects(deltaTime);
        }
    }
}
