using Flow.Sample.GamePlay.Components;
using UnityEngine;

namespace Flow.Sample.GamePlay.Entities
{
    [RequireComponent(typeof(MoveOnPathComponent))]
    [RequireComponent(typeof(CombatantComponent))]
    [RequireComponent(typeof(StatusComponent))]
    [RequireComponent(typeof(StatusEffectComponent))]
    public class EnemyEntity : BaseEntity
    {
        [field: SerializeField] public MoveOnPathComponent Move { get; private set; }
        [field: SerializeField] public CombatantComponent Combatant { get; private set; }
        [field: SerializeField] public StatusComponent Status { get; private set; }
        [field: SerializeField] public StatusEffectComponent StatusEffect { get; private set; }

        private void OnValidate()
        {
            if (Move == null)
                Move = GetComponent<MoveOnPathComponent>();

            if (Combatant == null)
                Combatant = GetComponent<CombatantComponent>();

            if (Status == null)
                Status = GetComponent<StatusComponent>();

            if (StatusEffect == null)
                StatusEffect = GetComponent<StatusEffectComponent>();
        }
    }
}