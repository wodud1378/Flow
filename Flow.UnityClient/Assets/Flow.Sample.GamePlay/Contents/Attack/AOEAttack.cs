using Flow.Sample.Data.StaticData.Attack;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Contents.Attack.Interfaces;
using Flow.Sample.GamePlay.Contents.Attack.Models;
using Flow.Sample.GamePlay.Systems;
using Flow.Sample.GamePlay.Systems.Models;
using UnityEngine;

namespace Flow.Sample.GamePlay.Contents.Attack
{
    public class AOEAttack : IAttack
    {
        private readonly CombatantComponent _owner;
        private readonly AOEAttackData _data;
        private readonly DetectSystem _detectSystem;
        private readonly DetectParamsProvider _detectParamsProvider;

        private readonly IAttackCondition _condition;
        private readonly IAttackViewSync _viewSync;

        private AttackContext _currentContext;

        public AttackData Data => _data;

        public AOEAttack(
            CombatantComponent owner,
            AOEAttackData data,
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

        ~AOEAttack()
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

            // Find first target for projectile destination
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
            // On hit, do AOE explosion at target position
            if (_currentContext.Targets.Count == 0)
                return;

            var primaryTarget = _currentContext.Targets[0];
            if (primaryTarget == null || !primaryTarget.IsValid)
                return;

            var explosionCenter = (Vector2)primaryTarget.transform.position;
            var explosionParams = new CircleParams(
                _data.explosionRadius,
                explosionCenter,
                new ContactFilter2D
                {
                    useLayerMask = true,
                    layerMask = _data.targeting.targetLayers
                }
            );

            using var explosionScope = _detectSystem.Detect<CombatantComponent>(explosionParams);

            // Clear and re-register all targets in explosion radius
            _currentContext.ClearTargets();
            foreach (var component in explosionScope.Detected)
            {
                var combatant = component.GetComponent<CombatantComponent>();
                _currentContext.RegisterTarget(combatant.Owner);
            }

            // Apply damage with explosion multiplier
            _currentContext.RunAttackWithMultiplier(_data.explosionDamageMultiplier);
        }

        private void OnViewEnd() => _currentContext.Dispose();
    }
}
