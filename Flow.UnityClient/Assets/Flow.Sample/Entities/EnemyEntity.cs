using Flow.Sample.GamePlay.Components;
using UnityEngine;

namespace Flow.Sample.Entities
{
    [RequireComponent(typeof(StatusComponent))]
    public class EnemyEntity : BaseEntity
    {
        [field:SerializeField] public StatusComponent Status { get; private set; }

        private void OnValidate()
        {
            if(Status == null)
                Status = GetComponent<StatusComponent>();
        }
    }
}