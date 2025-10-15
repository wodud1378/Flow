using Flow.Sample.GamePlay.Components;
using UnityEngine;

namespace Flow.Sample.Entities
{
    [RequireComponent(typeof(TowerComponent))]
    [RequireComponent(typeof(TargetingComponent))]
    [RequireComponent(typeof(DamageComponent))]
    public class TowerEntity : BaseEntity
    {
        private TowerComponent _towerComponent;
        private TargetingComponent _targetingComponent;
        private DamageComponent _damageComponent;
        
        public override void Initialize(int id)
        {
            base.Initialize(id);
            
            _towerComponent = GetComponent<TowerComponent>();
            _targetingComponent = GetComponent<TargetingComponent>();
            _damageComponent = GetComponent<DamageComponent>();
            
            // Set targeting range based on tower attack range
            if (_targetingComponent != null && _towerComponent != null)
            {
                _targetingComponent.TargetingRange = _towerComponent.AttackRange;
            }
        }
        
        public void Attack(BaseEntity target)
        {
            if (_towerComponent == null || !_towerComponent.CanAttack()) return;
            if (target == null || !target.IsValid) return;
            
            var projectile = CreateProjectile();
            if (projectile != null)
            {
                var projectileComp = projectile.GetComponent<ProjectileComponent>();
                if (projectileComp != null)
                {
                    projectileComp.Target = target;
                }
                
                // Copy damage from tower to projectile
                var projectileDamage = projectile.GetComponent<DamageComponent>();
                if (projectileDamage != null && _damageComponent != null)
                {
                    projectileDamage.BaseDamage = _damageComponent.BaseDamage;
                    projectileDamage.DamageMultiplier = _damageComponent.DamageMultiplier;
                    projectileDamage.DamageType = _damageComponent.DamageType;
                }
                
                _towerComponent.OnAttack();
            }
        }
        
        private GameObject CreateProjectile()
        {
            if (_towerComponent.ProjectilePrefab == null) return null;
            
            var firePoint = _towerComponent.FirePoint != null ? _towerComponent.FirePoint : transform;
            return Instantiate(_towerComponent.ProjectilePrefab, firePoint.position, firePoint.rotation);
        }
        
        public void Upgrade()
        {
            _towerComponent?.Upgrade();
        }
        
        public void Sell()
        {
            // Give gold back to player (handled by GameManager)
            Invalidate();
            Destroy(gameObject);
        }
    }
}
