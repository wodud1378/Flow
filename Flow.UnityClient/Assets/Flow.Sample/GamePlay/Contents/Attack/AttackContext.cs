using System.Collections.Generic;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Entities;
using Flow.Sample.GamePlay.Systems;

namespace Flow.Sample.GamePlay.Contents.Attack
{
    public class AttackContext
    {
        public IReadOnlyList<BaseEntity> Targets => _targets;
        public CombatantComponent Attacker { get; private set; }

        private readonly CombatSystem _system;
        private readonly List<BaseEntity> _targets = new();

        internal AttackContext(CombatSystem system)
        {
            _system = system;
        }

        public void SetAttacker(CombatantComponent component) => Attacker = component;

        public void RegisterTarget(BaseEntity target) => _targets.Add(target);

        public void RunAttack(int maxTargetCount)
        {
            if (Attacker == null || !Attacker.Owner.IsValid)
                return;

            int count = _targets.Count;
            for (int i = 0; i < count && i < maxTargetCount; ++i)
            {
                var target = _targets[i];
                _system.ApplyDamage(Attacker.Owner, target);
            }
            
            _targets.Clear();
        }

        public void Clear()
        {
            _targets.Clear();
        }

        public void Dispose()
        {
            Clear();

            _system.ReturnContext(this);
        }
    }
}