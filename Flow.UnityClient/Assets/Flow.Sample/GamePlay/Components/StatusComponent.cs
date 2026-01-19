using Flow.Sample.Data.StaticData.Enemy;
using Flow.Sample.GamePlay.Components.Interfaces;
using Flow.Sample.GamePlay.Entities;
using UnityEngine;

namespace Flow.Sample.GamePlay.Components
{
    public class StatusComponent : MonoBehaviour, IComponent
    {
        [field:SerializeField] public BaseEntity Owner { get; private set; }

        [field:SerializeField] public float MaxHp { get; set; }
        [field:SerializeField] public float Damage { get; set; }
        [field:SerializeField] public float CriticalRate { get; set; }
        [field:SerializeField] public float CriticalMultiplier { get; set; }
        [field:SerializeField] public float RemainHp { get; set; }

        [field:SerializeField] public int GoldReward { get; set; }
        [field:SerializeField] public int ScoreReward { get; set; }

        private void OnValidate()
        {
            if (Owner == null)
                Owner = GetComponent<BaseEntity>();

            RemainHp = MaxHp;
        }

        public void ApplyEnemyData(EnemyData data)
        {
            MaxHp = data.maxHp;
            RemainHp = data.maxHp;
            Damage = data.damage;
            CriticalRate = data.criticalRate;
            CriticalMultiplier = data.criticalMultiplier;
            GoldReward = data.goldReward;
            ScoreReward = data.scoreReward;
        }
    }
}