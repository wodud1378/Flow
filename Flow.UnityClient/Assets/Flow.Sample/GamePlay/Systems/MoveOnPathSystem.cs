using System;
using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Configs;
using Flow.Sample.GamePlay.Systems.Base;
using Flow.Sample.GamePlay.Systems.Interfaces;
using VContainer;

namespace Flow.Sample.GamePlay.Systems
{
    public class MoveOnPathSystem : BaseUpdateEntitySystem
    {
        protected override Type[] EntityFilter { get; } = { typeof(MoveOnPathComponent) };
        
        [Inject]
        public MoveOnPathSystem(
            IEntityContainer entityContainer,
            IComponentProvider componentCache, 
            IConfig config) 
            : base(entityContainer, componentCache, config.UpdateEntitySystemBufferSize)
        {
        }

        protected override void OnUpdateEntity(BaseEntity entity, int index, float deltaTime)
        {
            As<MoveOnPathComponent>(entity).UpdateMove(deltaTime);
        }
    }
}