using System.Collections.Generic;
using Flow.Sample.Entities;
using UnityEngine;

namespace Flow.Sample.GamePlay.Components
{
    public class TargetingComponent : MonoBehaviour, IComponent
    {
        [SerializeField] private float targetingRange = 10f;
        [SerializeField] private TargetingMode targetingMode = TargetingMode.Nearest;
        [SerializeField] private LayerMask targetLayers;
        
        private BaseEntity _currentTarget;
        private readonly List<BaseEntity> _targetsInRange = new();
        
        public float TargetingRange
        {
            get => targetingRange;
            set => targetingRange = value;
        }
        
        public TargetingMode TargetingMode
        {
            get => targetingMode;
            set => targetingMode = value;
        }
        
        public BaseEntity CurrentTarget => _currentTarget;
        public bool HasTarget => _currentTarget != null && _currentTarget.IsValid;
        public IReadOnlyList<BaseEntity> TargetsInRange => _targetsInRange;
        
        public void UpdateTargetsInRange()
        {
            _targetsInRange.Clear();
            
            var colliders = Physics.OverlapSphere(transform.position, targetingRange, targetLayers);
            foreach (var collider in colliders)
            {
                var entity = collider.GetComponent<BaseEntity>();
                if (entity != null && entity.IsValid)
                {
                    _targetsInRange.Add(entity);
                }
            }
        }
        
        public void SelectTarget()
        {
            UpdateTargetsInRange();
            
            if (_targetsInRange.Count == 0)
            {
                _currentTarget = null;
                return;
            }
            
            _currentTarget = targetingMode switch
            {
                TargetingMode.Nearest => GetNearestTarget(),
                TargetingMode.Farthest => GetFarthestTarget(),
                TargetingMode.Strongest => GetStrongestTarget(),
                TargetingMode.Weakest => GetWeakestTarget(),
                _ => _targetsInRange[0]
            };
        }
        
        private BaseEntity GetNearestTarget()
        {
            BaseEntity nearest = null;
            float minDistance = float.MaxValue;
            
            foreach (var target in _targetsInRange)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = target;
                }
            }
            
            return nearest;
        }
        
        private BaseEntity GetFarthestTarget()
        {
            BaseEntity farthest = null;
            float maxDistance = 0f;
            
            foreach (var target in _targetsInRange)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    farthest = target;
                }
            }
            
            return farthest;
        }
        
        private BaseEntity GetStrongestTarget()
        {
            BaseEntity strongest = null;
            float maxHealth = 0f;
            
            foreach (var target in _targetsInRange)
            {
                var health = target.GetComponent<HealthComponent>();
                if (health != null && health.MaxHealth > maxHealth)
                {
                    maxHealth = health.MaxHealth;
                    strongest = target;
                }
            }
            
            return strongest;
        }
        
        private BaseEntity GetWeakestTarget()
        {
            BaseEntity weakest = null;
            float minHealth = float.MaxValue;
            
            foreach (var target in _targetsInRange)
            {
                var health = target.GetComponent<HealthComponent>();
                if (health != null && health.CurrentHealth < minHealth)
                {
                    minHealth = health.CurrentHealth;
                    weakest = target;
                }
            }
            
            return weakest;
        }
    }
    
    public enum TargetingMode
    {
        Nearest,
        Farthest,
        Strongest,
        Weakest
    }
}
