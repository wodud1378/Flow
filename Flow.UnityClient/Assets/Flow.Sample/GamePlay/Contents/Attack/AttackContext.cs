using System.Collections.Generic;
using Flow.Sample.Entities;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Systems;

namespace Flow.Sample.GamePlay.Contents.Attack
{
    public class AttackContext
    {
        private readonly CombatSystem _system;
        private readonly Queue<BaseEntity> _targets = new();

        public AttackContext(CombatSystem system)
        {
            _system = system;
        }

        public void RegisterTarget(BaseEntity target) => _targets.Enqueue(target);

        public void RunAttack(CombatantComponent combatant)
        {
            while (_targets.Count > 0)
            {
                var target = _targets.Dequeue();
                _system.ApplyDamage(combatant.Owner, target);
            }
        }

        public void Clear()
        {
            _targets.Clear();
        }

        public void Dispose()
        {
            Clear();
            
            _system.Return(this);
        }
    }
}