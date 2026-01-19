using UnityEngine;

namespace Flow.Sample.Data.StaticData.Enemy
{
    [CreateAssetMenu(fileName = "FastEnemyData", menuName = "Flow/Enemy/Fast")]
    public class FastEnemyData : EnemyData
    {
        // Fast enemy: Low HP, High Speed
        // Default override in inspector or constructor

        private void OnEnable()
        {
            if (maxHp == 100f) maxHp = 50f;
            if (moveSpeed == 2f) moveSpeed = 4f;
            if (goldReward == 10) goldReward = 8;
            if (scoreReward == 50) scoreReward = 40;
        }
    }
}
