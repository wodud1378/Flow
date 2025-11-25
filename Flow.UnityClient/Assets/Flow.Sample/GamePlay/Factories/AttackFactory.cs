using System;
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
            return data switch
            {
                BasicAttackData basic => CreateBasic(owner, basic),
                _ => throw new ArgumentOutOfRangeException(nameof(data))
            };
        }

        private BasicAttack CreateBasic(CombatantComponent owner, BasicAttackData data)
        {
            return new BasicAttack(
                owner,
                data,
                _detectSystem,
                _detectParamsProvider,
                new CoolDown(data.cooldown),
                _viewSyncProvider.Provide(data)
            );
        }
    }
}