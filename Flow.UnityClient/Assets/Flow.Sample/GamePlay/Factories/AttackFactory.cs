using System;
using Flow.Sample.Data.StaticData;
using Flow.Sample.GamePlay.Contents.Attack.Delay;
using Flow.Sample.GamePlay.Contents.Attack.Interfaces;
using Flow.Sample.GamePlay.Systems;
using Flow.Sample.GamePlay.Systems.Models;
using Flow.Sample.Logic.Interfaces;
using UnityEngine;
using VContainer;

namespace Flow.Sample.GamePlay.Factories
{
    public class AttackFactory
    {
        private readonly DetectSystem _detectSystem;
        private readonly PoolSystem _poolSystem;
        private readonly ComponentCacheSystem _componentCacheSystem;

        [Inject]
        public AttackFactory(DetectSystem detectSystem, PoolSystem poolSystem, ComponentCacheSystem componentCacheSystem)
        {
            _detectSystem = detectSystem;
            _poolSystem = poolSystem;
            _componentCacheSystem = componentCacheSystem;
        }
        
        public IAttack Create(AttackData data)
        {
            throw new NotImplementedException();
        }

        private IAttackCondition GetCondition(AttackData data) => 
            new CoolDown(data.cooldown);

        private IDetectParams GetDetectParams(AttackData data, Vector2 position)
        {
            var filter = new ContactFilter2D
            {
                useTriggers = true,
                useLayerMask = true,
                layerMask = default,
            };

            return new CircleParams(data.range, position, filter);
        }
    }
}