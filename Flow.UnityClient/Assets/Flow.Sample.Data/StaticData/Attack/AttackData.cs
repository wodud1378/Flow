using Flow.Sample.Data.StaticData.Targeting;
using UnityEngine;

namespace Flow.Sample.Data.StaticData.Attack
{
    [CreateAssetMenu(fileName = "Attack", menuName = "Flow.Sample/Data/Attack")]
    public class AttackData : ScriptableObject
    {
        public float damageMultiplier;
        public float cooldown;
        public TargetingData targeting;
        public GameObject vfxPrefab;
    }
}
