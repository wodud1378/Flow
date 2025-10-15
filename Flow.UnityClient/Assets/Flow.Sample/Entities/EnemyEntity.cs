using Flow.Sample.GamePlay.Components;
using UnityEngine;

namespace Flow.Sample.Entities
{
    [RequireComponent(typeof(EnemyComponent))]
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(SpeedComponent))]
    public class EnemyEntity : BaseEntity
    {
        private EnemyComponent _enemyComponent;
        private HealthComponent _healthComponent;
        private SpeedComponent _speedComponent;
        
        public override void Initialize(int id)
        {
            base.Initialize(id);
            
            _enemyComponent = GetComponent<EnemyComponent>();
            _healthComponent = GetComponent<HealthComponent>();
            _speedComponent = GetComponent<SpeedComponent>();
            
            if (_healthComponent != null)
            {
                _healthComponent.OnDeath += OnDeath;
            }
        }
        
        public void SetPath(Vector3[] waypoints)
        {
            if (_enemyComponent != null)
            {
                _enemyComponent.SetPath(waypoints);
            }
        }
        
        public void MoveAlongPath(float deltaTime)
        {
            if (!IsValid || _enemyComponent == null || _speedComponent == null) return;
            if (_enemyComponent.HasReachedEnd) return;
            
            var targetPosition = _enemyComponent.GetCurrentTargetPosition();
            var direction = (targetPosition - transform.position).normalized;
            var moveDistance = _speedComponent.CurrentSpeed * deltaTime;
            
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveDistance);
            
            // Check if reached waypoint
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                _enemyComponent.MoveToNextWaypoint();
                
                if (_enemyComponent.HasReachedEnd)
                {
                    OnReachedEnd();
                }
            }
            
            // Rotate to face movement direction
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
        
        private void OnDeath()
        {
            if (!IsValid) return;
            
            // Give reward to player (handled by GameManager)
            Invalidate();
            
            // Play death animation/effects here
            Destroy(gameObject, 0.5f);
        }
        
        private void OnReachedEnd()
        {
            if (!IsValid) return;
            
            // Deal damage to player base (handled by GameManager)
            Invalidate();
            Destroy(gameObject);
        }
        
        public override void Invalidate()
        {
            base.Invalidate();
            
            if (_healthComponent != null)
            {
                _healthComponent.OnDeath -= OnDeath;
            }
        }
        
        public void ApplySlow(float slowPercent, float duration)
        {
            if (_speedComponent != null)
            {
                _speedComponent.ApplySlow(slowPercent);
                // Reset speed after duration (could use coroutine)
                Invoke(nameof(ResetSpeed), duration);
            }
        }
        
        private void ResetSpeed()
        {
            _speedComponent?.ResetSpeed();
        }
    }
}
