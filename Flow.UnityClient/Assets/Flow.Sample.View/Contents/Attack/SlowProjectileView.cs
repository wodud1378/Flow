using System;
using System.Collections.Generic;
using Flow.Sample.GamePlay.Contents.Attack;
using Flow.Sample.GamePlay.Contents.Attack.Interfaces;
using Flow.Sample.GamePlay.Contents.Attack.Models;
using Flow.Sample.GamePlay.Systems;
using UnityEngine;
using VContainer;

namespace Flow.Sample.View.Contents.Attack
{
    public class SlowProjectileView : MonoBehaviour, IAttackViewSync
    {
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private ParticleSystem slowEffectPrefab;
        [SerializeField] private float slowEffectDuration = 1f;
        [SerializeField] private Color slowTintColor = new Color(0.5f, 0.8f, 1f, 1f);

        public event Action<AttackContext, AttackViewEvent> OnViewEvent;

        private readonly List<Projectile> _launchedProjectiles = new();
        private readonly Queue<Projectile> _reachedProjectiles = new();

        private PoolSystem _poolSystem;

        [Inject]
        public void InjectDependencies(PoolSystem poolSystem)
        {
            _poolSystem = poolSystem;
        }

        public void ManualUpdate(float deltaTime)
        {
            while (_reachedProjectiles.Count > 0)
            {
                _launchedProjectiles.Remove(_reachedProjectiles.Dequeue());
            }

            foreach (var projectile in _launchedProjectiles)
            {
                projectile.ManualUpdate(deltaTime);
            }
        }

        public void Play(AttackContext context)
        {
            if (projectilePrefab == null)
                return;

            var startPosition = (Vector2)context.Attacker.Owner.transform.position;

            foreach (var target in context.Targets)
            {
                var projectile = _poolSystem.GetObject(projectilePrefab);
                projectile.transform.position = startPosition;

                _launchedProjectiles.Add(projectile);

                var targetEntity = target;
                projectile.Launch(startPosition, target);
                projectile.OnReachedTarget += () =>
                {
                    PlaySlowEffect(targetEntity.transform.position);

                    OnViewEvent?.Invoke(context, AttackViewEvent.Hit);
                    OnViewEvent?.Invoke(context, AttackViewEvent.End);

                    _reachedProjectiles.Enqueue(projectile);
                };
            }
        }

        private void PlaySlowEffect(Vector3 position)
        {
            if (slowEffectPrefab == null)
                return;

            var effect = _poolSystem.GetObject(slowEffectPrefab);
            effect.transform.position = position;

            var main = effect.main;
            main.startColor = slowTintColor;
            main.duration = slowEffectDuration;

            effect.Play();
        }
    }
}
