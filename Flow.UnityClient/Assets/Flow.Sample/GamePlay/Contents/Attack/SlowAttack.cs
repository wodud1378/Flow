using Flow.Sample.Data.StaticData.Attack;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Contents.Attack.Interfaces;
using Flow.Sample.GamePlay.Contents.Attack.Models;
using Flow.Sample.GamePlay.Systems;

namespace Flow.Sample.GamePlay.Contents.Attack
{
    public class SlowAttack : IAttack
    {
        private readonly CombatantComponent _owner;
        private readonly SlowAttackData _data;
        private readonly DetectSystem _detectSystem;
        private readonly DetectParamsProvider _detectParamsProvider;

        private readonly IAttackCondition _condition;
        private readonly IAttackViewSync _viewSync;

        private AttackContext _currentContext;

        public AttackData Data => _data;

        public SlowAttack(
            CombatantComponent owner,
            SlowAttackData data,
            DetectSystem detectSystem,
            DetectParamsProvider detectParamsProvider,
            IAttackCondition condition,
            IAttackViewSync viewSync)
        {
            _owner = owner;
            _data = data;
            _detectSystem = detectSystem;
            _detectParamsProvider = detectParamsProvider;
            _condition = condition;
            _viewSync = viewSync;

            _viewSync.OnViewEvent += OnViewEvent;
        }

        ~SlowAttack()
        {
            _viewSync.OnViewEvent -= OnViewEvent;
        }

        public bool CanExecute()
        {
            return _condition.Ready;
        }

        public void Execute(AttackContext context)
        {
            _currentContext = context;
            var detectParams = _detectParamsProvider.Provide(_owner, _data.targeting);
            using var scope = _detectSystem.Detect<CombatantComponent>(detectParams);

            foreach (var component in scope.Detected)
            {
                var combatant = component.GetComponent<CombatantComponent>();
                _currentContext.RegisterTarget(combatant.Owner);
            }

            _viewSync.Play(_currentContext);
        }

        public void Update(float deltaTime) => _condition.Update(deltaTime);

        private void OnViewEvent(AttackContext context, AttackViewEvent ev)
        {
            if (_currentContext != context)
                return;

            switch (ev)
            {
                case AttackViewEvent.Hit:
                    OnViewHitTiming();
                    break;
                case AttackViewEvent.End:
                    OnViewEnd();
                    break;
            }
        }

        private void OnViewHitTiming()
        {
            // Apply damage
            _currentContext.RunAttack(_data.targeting.maxTarget);

            // Apply slow effect to all targets
            foreach (var target in _currentContext.Targets)
            {
                if (target == null || !target.IsValid)
                    continue;

                if (!target.TryGetComponent<StatusEffectComponent>(out var effectComponent))
                    continue;

                var slowEffect = new StatusEffect(
                    StatusEffectType.Slow,
                    _data.slowAmount,
                    _data.slowDuration
                );

                effectComponent.AddEffect(slowEffect);
            }
        }

        private void OnViewEnd() => _currentContext.Dispose();
    }
}
