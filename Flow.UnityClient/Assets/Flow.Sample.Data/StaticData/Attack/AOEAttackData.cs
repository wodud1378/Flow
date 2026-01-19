using UnityEngine;

namespace Flow.Sample.Data.StaticData.Attack
{
    [CreateAssetMenu(fileName = "AOEAttackData", menuName = "Flow/Attack/AOE")]
    public class AOEAttackData : AttackData
    {
        [Header("AOE Settings")]
        public float explosionRadius = 2f;
        public float explosionDamageMultiplier = 0.5f;
    }
}
