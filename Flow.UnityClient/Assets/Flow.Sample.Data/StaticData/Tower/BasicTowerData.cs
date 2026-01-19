using UnityEngine;

namespace Flow.Sample.Data.StaticData.Tower
{
    [CreateAssetMenu(fileName = "BasicTowerData", menuName = "Flow/Tower/Basic")]
    public class BasicTowerData : TowerData
    {
        // Basic tower: Single target, moderate damage
        // Uses BasicAttackData for projectile attack
    }
}
