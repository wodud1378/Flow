using System.Collections.Generic;
using Flow.Sample.Entities;
using Flow.Sample.GamePlay.Components.Interfaces;
using Flow.Sample.GamePlay.Systems;
using Flow.Sample.GamePlay.Systems.Models;
using UnityEngine;

namespace Flow.Sample.GamePlay.Components.Attack
{
    public class BasicAttackComponent : MonoBehaviour, IAttackComponent
    {
        [SerializeField] private float range;
        [SerializeField] private LayerMask layerMask;
        
        public BaseEntity Owner { get; set; }
        public IDetectParams DetectParams { get; private set; }

        private IAttackFxComponent _fx;

        private readonly Queue<BaseEntity> _targets = new();

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
        
        public void BindFx(IAttackFxComponent fx)
        {
            _fx = fx;
        }

        public bool TryExecute(DetectScope<StatusComponent> scope)
        {
            if (scope.Detected.Length == 0)
                return false;
            
            foreach (var component in scope.Detected)
            {
                var status = component.GetComponent<StatusComponent>();
                _targets.Enqueue(status.Owner);
            }
            
            return true;
        }

        public void ApplyDamage(DetectScope<StatusComponent> scope, CombatSystem combatSystem)
        {
        }

        public void ManualUpdate(float deltaTime)
        {
            
        }
    }
}