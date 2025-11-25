using System;
using Flow.Sample.Data.StaticData;
using Flow.Sample.Data.StaticData.Attack;
using Flow.Sample.GamePlay.Contents.Attack.Interfaces;
using Flow.Sample.GamePlay.Systems;
using UnityEngine;
using VContainer;

namespace Flow.Sample.View.Contents.Attack
{
    public class AttackVIewSyncProvider : IAttackViewSyncProvider
    {
        private readonly PoolSystem _poolSystem;
        private readonly ComponentCacheSystem _componentCacheSystem;

        [Inject]
        public AttackVIewSyncProvider(PoolSystem poolSystem, ComponentCacheSystem componentCacheSystem)
        {
            _poolSystem = poolSystem;
            _componentCacheSystem = componentCacheSystem;
        }
        
        public IAttackViewSync Provide(AttackData data)
        {
            var prefab = data.vfxPrefab;
            if (!_componentCacheSystem.TryGetComponent(prefab, out IAttackViewSync t))
                throw new InvalidOperationException($"{prefab} does not implemented {nameof(IAttackViewSync)}");

            if (t is not MonoBehaviour monoBehaviour)
            {
                throw new InvalidOperationException(
                    $"Failed to cast component implementing {nameof(IAttackViewSync)} to {nameof(MonoBehaviour)}");
            }
            
            return (IAttackViewSync)_poolSystem.GetObject(monoBehaviour);
        }
    }
}