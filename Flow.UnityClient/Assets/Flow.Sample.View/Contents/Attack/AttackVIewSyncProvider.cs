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

        [Inject]
        public AttackVIewSyncProvider(PoolSystem poolSystem)
        {
            _poolSystem = poolSystem;
        }

        public IAttackViewSync Provide(AttackData data)
        {
            var prefab = data.vfxPrefab;
            if (!prefab.TryGetComponent<IAttackViewSync>(out var t))
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
