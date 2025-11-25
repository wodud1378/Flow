using UnityEngine;

namespace Flow.Sample.Data.StaticData.Targeting
{
    [CreateAssetMenu(fileName = "BasicAttack", menuName = "Flow.Sample/Data/Targeting Data/Box")]
    public class BoxTargetingData : TargetingData
    {
        public float angle;
        public Vector2 size;
    }
}