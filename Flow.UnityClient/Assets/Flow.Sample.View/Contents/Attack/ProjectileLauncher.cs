using System;
using System.Collections.Generic;
using Flow.Sample.GamePlay.Contents.Attack;
using Flow.Sample.GamePlay.Contents.Attack.Interfaces;
using Flow.Sample.GamePlay.Contents.Attack.Models;
using Flow.Sample.GamePlay.Systems;
using Flow.Sample.Logic;
using UnityEngine;
using VContainer;

namespace Flow.Sample.View.Contents.Attack
{
    public class ProjectileLauncher : MonoBehaviour, IAttackViewSync
    {
        [SerializeField] private Projectile prefab;

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

            _launchedProjectiles.ForEach(x => x.ManualUpdate(deltaTime));
        }

        public void Play(AttackContext context)
        {
            if (prefab == null)
                return;

            var startPosition = (Vector2)context.Attacker.Owner.transform.position;
            foreach (var target in context.Targets)
            {
                var projectile = _poolSystem.GetObject(prefab, p => { p.transform.position = startPosition; });

                _launchedProjectiles.Add(projectile);

                projectile.Launch(startPosition, target);
                projectile.OnReachedTarget += () =>
                {
                    OnViewEvent?.Invoke(context, AttackViewEvent.Hit);
                    OnViewEvent?.Invoke(context, AttackViewEvent.End);

                    _reachedProjectiles.Enqueue(projectile);
                };
            }
        }
    }
}