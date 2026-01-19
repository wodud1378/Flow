using UnityEngine;

namespace Flow.Sample.Data.StaticData.Enemy
{
    [CreateAssetMenu(fileName = "TankEnemyData", menuName = "Flow/Enemy/Tank")]
    public class TankEnemyData : EnemyData
    {
        // Tank enemy: High HP, Low Speed
        // Default override in inspector or constructor

        private void OnEnable()
        {
            if (maxHp == 100f) maxHp = 300f;
            if (moveSpeed == 2f) moveSpeed = 1f;
            if (goldReward == 10) goldReward = 25;
            if (scoreReward == 50) scoreReward = 100;
        }
    }
}
