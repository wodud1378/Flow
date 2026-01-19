using UnityEngine;

namespace Flow.Sample.Data.StaticData.Enemy
{
    public abstract class EnemyData : ScriptableObject
    {
        [Header("Basic Stats")]
        public float maxHp = 100f;
        public float moveSpeed = 2f;
        public float damage = 10f;

        [Header("Rewards")]
        public int goldReward = 10;
        public int scoreReward = 50;

        [Header("Combat")]
        public float criticalRate = 0f;
        public float criticalMultiplier = 1.5f;
    }
}
