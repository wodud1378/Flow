using UnityEngine;

namespace Flow.Sample.Data.StaticData.Targeting
{
    [CreateAssetMenu(fileName = "BasicAttack", menuName = "Flow.Sample/Data/Targeting Data/Circle")]
    public class CircleTargetingData : TargetingData
    {
        public float radius;
    }
}