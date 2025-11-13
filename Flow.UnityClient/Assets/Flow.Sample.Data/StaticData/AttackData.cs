using UnityEngine;

namespace Flow.Sample.Data.StaticData
{
    [CreateAssetMenu(fileName = "Attack", menuName = "Flow.Sample/Data/Attack")]
    public class AttackData : ScriptableObject
    {
        public float damageMultiplier;
        public float cooldown;
        public float range;
        public int attackCount;
        public int maxTarget;
        public GameObject vfxPrefab;
    }
}
