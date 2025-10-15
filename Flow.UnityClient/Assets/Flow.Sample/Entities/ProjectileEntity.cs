using Flow.Sample.GamePlay.Components;
using UnityEngine;

namespace Flow.Sample.Entities
{
    [RequireComponent(typeof(ProjectileComponent))]
    public class ProjectileEntity : BaseEntity
    {
        private ProjectileComponent _projectileComponent;
        private DamageComponent _damageComponent;
        
        public override void Initialize(int id)
        {
            base.Initialize(id);
            
            _projectileComponent = GetComponent<ProjectileComponent>();
            _damageComponent = GetComponent<DamageComponent>();
        }
        
        public void UpdateMovement(float deltaTime)
        {
            if (!IsValid || _projectileComponent == null) return;
            
            // Check if projectile has expired
            if (_projectileComponent.IsExpired())
            {
                DestroyProjectile();
                return;
            }
            
            // Move towards target or in direction
            if (_projectileComponent.FollowTarget && _projectileComponent.Target != null)
            {
                if (!_projectileComponent.Target.IsValid)
                {
                    DestroyProjectile();
                    return;
                }
                
                MoveTowardsTarget(deltaTime);
            }
            else
            {
                MoveStraight(deltaTime);
            }
        }
        
        private void MoveTowardsTarget(float deltaTime)
        {
            var target = _projectileComponent.Target;
            var direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * _projectileComponent.Speed * deltaTime;
            
            // Rotate to face target
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
            
            // Check if hit target
            if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
            {
                OnHitTarget();
            }
        }
        
        private void MoveStraight(float deltaTime)
        {
            transform.position += transform.forward * _projectileComponent.Speed * deltaTime;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!IsValid) return;
            
            var entity = other.GetComponent<BaseEntity>();
            if (entity != null && entity.IsValid)
            {
                // Check if it's an enemy (could use tags or layers)
                if (entity is EnemyEntity)
                {
                    _projectileComponent.Target = entity;
                    OnHitTarget();
                }
            }
        }
        
        private void OnHitTarget()
        {
            if (!IsValid) return;
            
            _projectileComponent.OnHit();
            DestroyProjectile();
        }
        
        private void DestroyProjectile()
        {
            if (!IsValid) return;
            
            Invalidate();
            
            // Play hit effect/particle here
            Destroy(gameObject, 0.1f);
        }
    }
}
