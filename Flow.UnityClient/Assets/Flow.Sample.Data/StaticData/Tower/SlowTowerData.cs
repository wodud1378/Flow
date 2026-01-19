using UnityEngine;

namespace Flow.Sample.Data.StaticData.Tower
{
    [CreateAssetMenu(fileName = "SlowTowerData", menuName = "Flow/Tower/Slow")]
    public class SlowTowerData : TowerData
    {
        [Header("Slow Settings")]
        public float slowAmount = 0.5f;
        public float slowDuration = 2f;

        // Slow tower: Applies slow debuff to enemies
        // Uses SlowAttackData
    }
}
