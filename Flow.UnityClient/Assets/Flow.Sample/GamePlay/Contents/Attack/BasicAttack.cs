using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Contents.Attack.Interfaces;
using Flow.Sample.GamePlay.Contents.Attack.Models;
using Flow.Sample.GamePlay.Systems;
using Flow.Sample.GamePlay.Systems.Models;

namespace Flow.Sample.GamePlay.Contents.Attack
{
    public class BasicAttack : IAttack
    {
        private readonly DetectSystem _detectSystem;

        private readonly IAttackCondition _condition;
        private readonly IDetectParams _detectParams;
        private readonly IAttackViewSync _viewSync;

        private AttackContext _currentContext;

        public BasicAttack(
            DetectSystem detectSystem,
            IAttackCondition condition,
            IDetectParams detectParams,
            IAttackViewSync viewSync)
        {
            _detectSystem = detectSystem;
            _condition = condition;
            _detectParams = detectParams;
            _viewSync = viewSync;

            _viewSync.OnViewEvent += OnViewEvent;
        }

        ~BasicAttack()
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
            using var scope = _detectSystem.Detect<CombatantComponent>(_detectParams);
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
            if(_currentContext != context)
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
        
        private void OnViewHitTiming() => _currentContext.RunAttack();

        private void OnViewEnd() => _currentContext.Dispose();
    }
}