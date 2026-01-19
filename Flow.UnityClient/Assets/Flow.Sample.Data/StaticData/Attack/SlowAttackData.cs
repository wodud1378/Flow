using UnityEngine;

namespace Flow.Sample.Data.StaticData.Attack
{
    [CreateAssetMenu(fileName = "SlowAttackData", menuName = "Flow/Attack/Slow")]
    public class SlowAttackData : AttackData
    {
        [Header("Slow Settings")]
        public float slowAmount = 0.5f;
        public float slowDuration = 2f;
    }
}
