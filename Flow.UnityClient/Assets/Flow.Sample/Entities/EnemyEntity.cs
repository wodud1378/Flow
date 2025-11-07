using Flow.Sample.GamePlay.Components;
using UnityEngine;

namespace Flow.Sample.Entities
{
    [RequireComponent(typeof(MoveOnPathComponent), typeof(CombatantComponent), typeof(StatusComponent))]
    public class EnemyEntity : BaseEntity
    {
        [field: SerializeField] public MoveOnPathComponent Move { get; private set; }
        [field: SerializeField] public CombatantComponent Combatant { get; private set; }
        [field: SerializeField] public StatusComponent Status { get; private set; }

        private void OnValidate()
        {
            if (Move == null)
                Move = GetComponent<MoveOnPathComponent>();

            if (Combatant == null)
                Combatant = GetComponent<CombatantComponent>();

            if (Status == null)
                Status = GetComponent<StatusComponent>();
        }
    }
}