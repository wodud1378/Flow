using Flow.Sample.GamePlay.Components;
using UnityEngine;

namespace Flow.Sample.Entities
{
    [RequireComponent(typeof(CombatantComponent))]
    public class EnemyEntity : BaseEntity
    {
        [field:SerializeField] public CombatantComponent Combatant { get; private set; }
        [field:SerializeField] public StatusComponent Status { get; private set; }

        private void OnValidate()
        {
            if(Combatant == null)
                Combatant = GetComponent<CombatantComponent>();
            
            if(Status == null)
                Status = GetComponent<StatusComponent>();
        }
    }
}