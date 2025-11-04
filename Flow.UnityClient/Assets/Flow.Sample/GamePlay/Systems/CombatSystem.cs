using System;
using System.Collections.Generic;
using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Configs;
using Flow.Sample.GamePlay.Contents.Attack;
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
        protected override Type[] EntityFilter { get; } = { typeof(CombatantComponent) };

        private readonly DamageCalculator _damageCalculator;
        private readonly GameEvents _events;
        private readonly Queue<AttackContext> _contextPool;

        [Inject]
        public CombatSystem(
            IEntityContainer entityContainer,
            IComponentProvider componentCache,
            DamageCalculator damageCalculator,
            GameEvents events,
            IBufferConfig bufferConfig)
            : base(entityContainer, componentCache, bufferConfig.UpdateEntitySystemBufferSize)
        {
            _damageCalculator = damageCalculator;
            _events = events;
        }

        protected override void OnUpdateEntity(BaseEntity entity, int index, float deltaTime)
        {
            var combatant = entity.GetComponent<CombatantComponent>();
            if (!combatant.IsAlive)
                return;
            
            combatant.ManualUpdate(deltaTime);
            foreach (var attack in combatant.Attacks)
            {
                if (!attack.CanExecute())
                    continue;

                var context = GetAttackContext();
                attack.Execute(context);
            }
        }

        public IDamage CalculateDamage(StatusComponent status) =>
            _damageCalculator.CalculateDamage(status);

        public void ApplyDamage(BaseEntity attacker, BaseEntity victim)
        {
            var damage = CalculateDamage(attacker.GetComponent<CombatantComponent>().Status);
            ApplyDamage(attacker, victim, damage);
        }

        public void ApplyDamage(BaseEntity attacker, BaseEntity victim, IDamage damage)
        {
            if (!IsValidCombatant(attacker, out _) || !IsValidCombatant(victim, out var combatant))
                return;

            combatant.ApplyDamage(damage.Value);
            _events.DamagedStream.OnNext(new Damaged(
                attacker, victim, damage
            ));
        }

        private bool IsValidCombatant(BaseEntity entity, out CombatantComponent combatant)
        {
            if (entity == null || !entity.IsValid)
            {
                combatant = null;
                return false;
            }
            
            combatant = entity.GetComponent<CombatantComponent>();
            return combatant != null && combatant.IsAlive;
        }

        private AttackContext GetAttackContext()
        {
            var context = _contextPool.Count > 0 ? _contextPool.Dequeue() : new AttackContext(this);
            context.Clear();
            
            return context;
        }
        
        public void Return(AttackContext context) => _contextPool.Enqueue(context);
    }
}