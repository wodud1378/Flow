using Flow.Sample.Data.StaticData.Targeting;
using UnityEngine;

namespace Flow.Sample.Data.StaticData.Attack
{
    public abstract class AttackData : ScriptableObject
    {
        public float cooldown;
        public float damageMultiplier;
        public TargetingData targeting;
        public GameObject vfxPrefab;
    }
}
