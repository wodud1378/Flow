using UnityEngine;

namespace Flow.Sample.Data.StaticData.Tower
{
    [CreateAssetMenu(fileName = "AOETowerData", menuName = "Flow/Tower/AOE")]
    public class AOETowerData : TowerData
    {
        [Header("AOE Settings")]
        public float explosionRadius = 2f;

        // AOE tower: Area damage on impact
        // Uses AOEAttackData
    }
}
