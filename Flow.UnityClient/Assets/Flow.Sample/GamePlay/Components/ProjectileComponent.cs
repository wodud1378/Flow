using Flow.Sample.Entities;
using UnityEngine;

namespace Flow.Sample.GamePlay.Components
{
    public class ProjectileComponent : MonoBehaviour, IComponent
    {
        [Header("Projectile Settings")]
        [SerializeField] private float speed = 20f;
        [SerializeField] private float lifetime = 5f;
        [SerializeField] private bool followTarget = true;
        [SerializeField] private float explosionRadius = 0f;
        
        private BaseEntity _target;
        private float _spawnTime;
        
        public float Speed
        {
            get => speed;
            set => speed = value;
        }
        
        public float Lifetime => lifetime;
        public bool FollowTarget => followTarget;
        public float ExplosionRadius => explosionRadius;
        public bool IsAreaDamage => explosionRadius > 0;
        
        public BaseEntity Target
        {
            get => _target;
            set => _target = value;
        }
        
        private void Start()
        {
            _spawnTime = Time.time;
        }
        
        public bool IsExpired()
        {
            return Time.time - _spawnTime >= lifetime;
        }
        
        public void OnHit()
        {
            if (IsAreaDamage)
            {
                ApplyAreaDamage();
            }
            else
            {
                ApplyDirectDamage();
            }
        }
        
        private void ApplyDirectDamage()
        {
            if (_target == null || !_target.IsValid) return;
            
            var damage = GetComponent<DamageComponent>();
            var targetHealth = _target.GetComponent<HealthComponent>();
            
            if (damage != null && targetHealth != null)
            {
                targetHealth.TakeDamage(damage.CalculateDamage());
            }
        }
        
        private void ApplyAreaDamage()
        {
            var damage = GetComponent<DamageComponent>();
            if (damage == null) return;
            
            var colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (var collider in colliders)
            {
                var entity = collider.GetComponent<BaseEntity>();
                if (entity != null && entity.IsValid)
                {
                    var health = entity.GetComponent<HealthComponent>();
                    if (health != null)
                    {
                        health.TakeDamage(damage.CalculateDamage());
                    }
                }
            }
        }
    }
}
