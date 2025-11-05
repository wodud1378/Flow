using System;
using System.Collections.Generic;
using Flow.Sample.Entities;
using Flow.Sample.GamePlay.Components.Interfaces;
using Flow.Sample.GamePlay.Contents.Attack.Interfaces;
using UnityEngine;

namespace Flow.Sample.GamePlay.Components
{
    [RequireComponent(typeof(StatusComponent))]
    public class CombatantComponent : MonoBehaviour, IComponent
    {
        public IReadOnlyList<IAttack> Attacks => _attacks;
        
        [field:SerializeField] public BaseEntity Owner { get; private set; }
        [field:SerializeField] public StatusComponent Status { get; private set; }
        public bool IsAlive => Status.RemainHp > 0;
        
        private readonly List<IAttack> _attacks = new();

        private void OnValidate()
        {
            if(Owner == null)
                Owner = GetComponent<BaseEntity>();
            
            if(Status == null)
                Status = GetComponent<StatusComponent>();
        }

        public void AddAttack(IAttack attack) => _attacks.Add(attack);

        public void RemoveAttack(IAttack attack) => _attacks.Remove(attack);

        public void ManualUpdate(float deltaTime)
        {
            foreach (var attack in _attacks)
            {
                attack.Update(deltaTime);
            }
        }
        
        public void ApplyDamage(float damage) => Status.RemainHp = Mathf.Max(0, Status.RemainHp - damage);
    }
}