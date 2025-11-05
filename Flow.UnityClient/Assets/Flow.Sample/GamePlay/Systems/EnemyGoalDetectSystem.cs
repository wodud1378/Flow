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
    public class EnemyGoalDetectSystem : BaseUpdateEntitySystem
    {
        protected override Type[] EntityFilter { get; } = { typeof(MoveOnPathComponent), typeof(StatusComponent) };
        
        private readonly PlayerHpSystem _playerHpSystem;

        [Inject]
        public EnemyGoalDetectSystem(
            IEntityContainer entityContainer,
            IComponentProvider componentCache,
            IConfig config,
            PlayerHpSystem playerHpSystem)
            : base(entityContainer, componentCache, config.UpdateEntitySystemBufferSize)
        {
            _playerHpSystem = playerHpSystem;
        }

        protected override void OnUpdateEntity(BaseEntity entity, int index, float deltaTime)
        {
            var move = As<MoveOnPathComponent>(entity);
            if (!move.ReachedToEnd)
                return;

            var status = As<StatusComponent>(entity);
            _playerHpSystem.RequestDecrease(status.RemainHp);
        }
    }
}