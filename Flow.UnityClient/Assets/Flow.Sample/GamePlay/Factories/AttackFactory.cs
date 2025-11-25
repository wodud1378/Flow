using Flow.Sample.Data.StaticData.Attack;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Contents.Attack;
using Flow.Sample.GamePlay.Contents.Attack.Delay;
using Flow.Sample.GamePlay.Contents.Attack.Interfaces;
using Flow.Sample.GamePlay.Systems;
using VContainer;

namespace Flow.Sample.GamePlay.Factories
{
    public class AttackFactory
    {
        private readonly DetectSystem _detectSystem;
        private readonly DetectParamsProvider _detectParamsProvider;
        private readonly IAttackViewSyncProvider _viewSyncProvider;

        [Inject]
        public AttackFactory(
            DetectSystem detectSystem,
            DetectParamsProvider detectParamsProvider,
            IAttackViewSyncProvider viewSyncProvider)
        {
            _detectSystem = detectSystem;
            _detectParamsProvider = detectParamsProvider;
            _viewSyncProvider = viewSyncProvider;
        }

        public IAttack Create(CombatantComponent owner, AttackData data)
        {
            return new BasicAttack(
                owner,
                data,
                _detectSystem,
                _detectParamsProvider,
                GetCondition(data),
                _viewSyncProvider.Provide(data)
            );
        }

        private IAttackCondition GetCondition(AttackData data) =>
            new CoolDown(data.cooldown);
    }
}