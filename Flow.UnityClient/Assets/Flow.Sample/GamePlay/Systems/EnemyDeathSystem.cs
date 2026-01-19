using System;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Entities;
using Flow.Sample.GamePlay.Events;
using Flow.Sample.GamePlay.Logics.Models;
using Flow.Sample.GamePlay.Systems.Base;
using R3;
using VContainer;

namespace Flow.Sample.GamePlay.Systems
{
    public class EnemyDeathSystem : BaseUpdateSystem
    {
        private readonly EnemyEvents _enemyEvents;
        private readonly ResourceSystem _resourceSystem;
        private readonly IDisposable _subscription;

        [Inject]
        public EnemyDeathSystem(CombatEvents combatEvents, EnemyEvents enemyEvents, ResourceSystem resourceSystem)
        {
            _enemyEvents = enemyEvents;
            _resourceSystem = resourceSystem;

            _subscription = combatEvents.OnDamaged.Subscribe(OnDamaged);
        }

        ~EnemyDeathSystem()
        {
            _subscription?.Dispose();
        }

        private void OnDamaged(Damaged damaged)
        {
            if (damaged.Victim == null || !damaged.Victim.IsValid)
                return;

            if (damaged.Victim is not EnemyEntity enemy)
                return;

            if (!enemy.TryGetSystemComponent<CombatantComponent>(out var combatant))
                return;

            if (combatant.IsAlive)
                return;

            var status = enemy.GetSystemComponent<StatusComponent>();

            _resourceSystem.AddGold(status.GoldReward);
            _resourceSystem.AddScore(status.ScoreReward);

            _enemyEvents.EnemyKilledStream.OnNext(enemy);
            enemy.DestroySelf();
        }

        protected override void OnUpdate(float deltaTime)
        {
            // Event-driven, no update needed
        }
    }
}
