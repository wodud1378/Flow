using System;
using Flow.Sample.GamePlay.Contents.Attack;
using Flow.Sample.GamePlay.Contents.Attack.Interfaces;
using Flow.Sample.GamePlay.Contents.Attack.Models;
using Flow.Sample.GamePlay.Systems;
using UnityEngine;
using VContainer;

namespace Flow.Sample.View.Contents.Attack
{
    public class AOEExplosionView : MonoBehaviour, IAttackViewSync
    {
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private ParticleSystem explosionPrefab;
        [SerializeField] private float explosionRadius = 2f;

        public event Action<AttackContext, AttackViewEvent> OnViewEvent;

        private PoolSystem _poolSystem;
        private Projectile _activeProjectile;
        private AttackContext _currentContext;

        [Inject]
        public void InjectDependencies(PoolSystem poolSystem)
        {
            _poolSystem = poolSystem;
        }

        public void ManualUpdate(float deltaTime)
        {
            _activeProjectile?.ManualUpdate(deltaTime);
        }

        public void Play(AttackContext context)
        {
            if (projectilePrefab == null || context.Targets.Count == 0)
                return;

            _currentContext = context;
            var startPosition = (Vector2)context.Attacker.Owner.transform.position;
            var target = context.Targets[0];

            _activeProjectile = _poolSystem.GetObject(projectilePrefab);
            _activeProjectile.Launch(startPosition, target);
            _activeProjectile.OnReachedTarget += OnProjectileReached;
        }

        private void OnProjectileReached()
        {
            if (_activeProjectile != null)
            {
                PlayExplosion(_activeProjectile.transform.position);
                _activeProjectile.OnReachedTarget -= OnProjectileReached;
                _activeProjectile.Deactivate();
                _activeProjectile = null;
            }

            OnViewEvent?.Invoke(_currentContext, AttackViewEvent.Hit);
            OnViewEvent?.Invoke(_currentContext, AttackViewEvent.End);
        }

        private void PlayExplosion(Vector3 position)
        {
            if (explosionPrefab == null)
                return;

            var explosion = _poolSystem.GetObject(explosionPrefab);
            explosion.transform.position = position;

            var main = explosion.main;
            main.startSize = explosionRadius * 2f;

            explosion.Play();
        }
    }
}
