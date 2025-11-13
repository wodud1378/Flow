using System.Collections.Generic;
using Flow.Sample.Entities;
using Flow.Sample.GamePlay.Components;
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

        public void RunAttack()
        {
            if (Attacker == null || !Attacker.Owner.IsValid)
                return;
            
            _targets.ForEach(x => _system.ApplyDamage(Attacker.Owner, x));
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