using System;
using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Components.Interfaces;
using Flow.Sample.GamePlay.Components.Status;
using Flow.Sample.GamePlay.Events;
using Flow.Sample.GamePlay.Systems.Base;
using Flow.Sample.GamePlay.Systems.Interfaces;
using Flow.Sample.Logic;
using Flow.Sample.Logic.Models;
using VContainer;

namespace Flow.Sample.GamePlay.Systems
{
    public class CombatSystem : BaseUpdateEntitySystem
    {
        protected override Type[] EntityFilter { get; } = { typeof(IAttackComponent) };

        private readonly DetectSystem _detectSystem;
        private readonly DamageCalculator _damageCalculator;
        private readonly GameEvents _events;

        [Inject]
        public CombatSystem(
            DetectSystem detectSystem, 
            IEntityContainer entityContainer,
            IComponentProvider componentCache,
            DamageCalculator damageCalculator,
            GameEvents events) 
            : base(entityContainer, componentCache)
        {
            _detectSystem = detectSystem;
            _damageCalculator = damageCalculator;
            _events = events;
        }

        protected override void OnUpdateEntity(BaseEntity entity, int index, float deltaTime)
        {
            var attack = As<IAttackComponent>(entity);
            using var scope = _detectSystem.Detect<StatusComponent>(attack.DetectParams);
            if (!attack.TryExecute(scope))
                return;
        }

        public IDamage CalculateDamage(StatusComponent status)
        {
            var isCritical = UnityEngine.Random.Range(0f, 1f) < status.CriticalRate;
            var multiplier = isCritical ? status.CriticalMultiplier : 1f;
            var value = status.Damage * multiplier;
            
            return isCritical 
                ? new CriticalDamage(value)
                : new Damage(value);
        }
    }
}