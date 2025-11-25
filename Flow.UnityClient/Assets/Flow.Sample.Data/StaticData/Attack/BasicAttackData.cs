using UnityEngine;

namespace Flow.Sample.Data.StaticData.Attack
{
    [CreateAssetMenu(fileName = "BasicAttack", menuName = "Flow.Sample/Data/Attack/Basic Attack")]
    public class BasicAttackData : AttackData
    {
        public float cooldown;
    }
}