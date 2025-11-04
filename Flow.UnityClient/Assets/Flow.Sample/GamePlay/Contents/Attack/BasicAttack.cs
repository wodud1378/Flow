using System.Collections.Generic;
using Flow.Sample.Entities;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Contents.Attack.Interfaces;
using Flow.Sample.GamePlay.Systems;
using Flow.Sample.GamePlay.Systems.Models;

namespace Flow.Sample.GamePlay.Contents.Attack
{
    public class BasicAttack : IAttack
    {
        private readonly CombatantComponent _owner;
        private readonly DetectSystem _detectSystem;

        private readonly IAttackCondition _condition;
        private readonly IDetectParams _detectParams;
        private readonly IAttackViewSync _viewSync;

        private AttackContext _currentContext;

        public BasicAttack(
            CombatantComponent owner,
            DetectSystem detectSystem,
            IAttackCondition condition,
            IDetectParams detectParams,
            IAttackViewSync viewSync)
        {
            _owner = owner;
            _detectSystem = detectSystem;
            _condition = condition;
            _detectParams = detectParams;
            _viewSync = viewSync;

            _viewSync.OnHitTiming += OnViewHitTiming;
            _viewSync.OnEnd += OnViewEnd;
        }

        ~BasicAttack()
        {
            _viewSync.OnHitTiming -= OnViewHitTiming;
            _viewSync.OnEnd -= OnViewEnd;
        }

        public bool CanExecute()
        {
            return _condition.Ready;
        }

        public void Execute(AttackContext context)
        {
            _currentContext = context;
            using var scope = _detectSystem.Detect<CombatantComponent>(_detectParams);
            foreach (var component in scope.Detected)
            {
                var combatant = component.GetComponent<CombatantComponent>();
                _currentContext.RegisterTarget(combatant.Owner);
            }

            _viewSync.Play();
        }

        public void Update(float deltaTime) => _condition.Update(deltaTime);

        private void OnViewHitTiming() => _currentContext.RunAttack(_owner);

        private void OnViewEnd() => _currentContext.Dispose();
    }
}