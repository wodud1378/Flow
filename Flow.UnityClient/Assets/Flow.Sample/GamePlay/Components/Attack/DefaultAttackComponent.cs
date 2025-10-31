using System;
using Flow.Sample.Entities;
using Flow.Sample.GamePlay.Components.Interfaces;
using Flow.Sample.GamePlay.Systems;
using Flow.Sample.GamePlay.Systems.Models;
using UnityEngine;

namespace Flow.Sample.GamePlay.Components.Attack
{
    public class DefaultAttackComponent : MonoBehaviour, IAttack
    {
        [field:SerializeField] public float Damage { get; private set; }
        [field:SerializeField] public float Critical { get; private set; }
        public BaseEntity Owner { get; private set; }

        [SerializeField] private float range;
        [SerializeField] private LayerMask layerMask;
        
        public IDetectParams DetectParams { get; private set; }

        public void Initialize(BaseEntity owner)
        {
            Owner = owner;
            
            DetectParams = new CircleParams(
                range,
                transform.position,
                new ContactFilter2D
                {
                    useLayerMask = true,
                    layerMask = layerMask,
                }
            );
        }

        public bool CanExecute(BaseEntity attacker, DetectScope<HealthComponent> scope)
        {
            return scope.Detected.Length > 0;
        }
        
        public void Execute(BaseEntity attacker, DetectScope<HealthComponent> scope)
        {
            foreach (var component in scope.Detected)
            {
                var health = (HealthComponent)component;
                health.Decrease(Damage);
            }
        }

    }
}