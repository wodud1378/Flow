using UnityEngine;

namespace Flow.Sample.Data.StaticData.Enemy
{
    [CreateAssetMenu(fileName = "BasicEnemyData", menuName = "Flow/Enemy/Basic")]
    public class BasicEnemyData : EnemyData
    {
        // Basic enemy uses default stats
        // HP: 100, Speed: 2, Gold: 10, Score: 50
    }
}
