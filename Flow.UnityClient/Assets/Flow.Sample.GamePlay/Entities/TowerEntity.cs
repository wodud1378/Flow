using Flow.Sample.GamePlay.Components;
using UnityEngine;

namespace Flow.Sample.GamePlay.Entities
{
    [RequireComponent(typeof(SizeComponent))]
    [RequireComponent(typeof(CombatantComponent))]
    public class TowerEntity : BaseEntity
    {
        [field: SerializeField] public SizeComponent Size { get; private set; }
        [field: SerializeField] public CombatantComponent Combatant { get; private set; }

        private void OnValidate()
        {
            if (Size == null)
                Size = GetComponent<SizeComponent>();

            if (Combatant == null)
                Combatant = GetComponent<CombatantComponent>();
        }
    }
}