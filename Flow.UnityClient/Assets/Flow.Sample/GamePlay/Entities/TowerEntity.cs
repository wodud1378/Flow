using Flow.Sample.GamePlay.Components;
using UnityEngine;

namespace Flow.Sample.GamePlay.Entities
{
    [RequireComponent(typeof(CombatantComponent))]
    public class TowerEntity : BaseEntity
    {
        [field:SerializeField] public CombatantComponent Combatant { get; private set; }

        private void OnValidate()
        {
            if(Combatant == null)
                Combatant = GetComponent<CombatantComponent>();
        }
    }
}