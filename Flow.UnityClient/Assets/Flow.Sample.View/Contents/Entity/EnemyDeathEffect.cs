using System;
using Flow.Sample.GamePlay.Entities;
using Flow.Sample.GamePlay.Events;
using Flow.Sample.GamePlay.Systems;
using R3;
using UnityEngine;
using VContainer;

namespace Flow.Sample.View.Contents.Entity
{
    public class EnemyDeathEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem deathEffectPrefab;
        [SerializeField] private float effectDuration = 1f;

        private PoolSystem _poolSystem;
        private IDisposable _subscription;

        [Inject]
        public void Initialize(EnemyEvents enemyEvents, PoolSystem poolSystem)
        {
            _poolSystem = poolSystem;
            _subscription = enemyEvents.OnEnemyKilled.Subscribe(OnEnemyKilled);
        }

        private void OnDestroy()
        {
            _subscription?.Dispose();
        }

        private void OnEnemyKilled(EnemyEntity enemy)
        {
            if (deathEffectPrefab == null)
                return;

            if (!enemy.IsValid)
                return;

            var position = enemy.transform.position;
            PlayDeathEffect(position);
        }

        private void PlayDeathEffect(Vector3 position)
        {
            var effect = _poolSystem.GetObject(deathEffectPrefab);
            effect.transform.position = position;

            var main = effect.main;
            main.duration = effectDuration;

            effect.Play();
        }
    }
}
