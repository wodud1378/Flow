using UnityEngine;

namespace Flow.Sample.Data.StaticData.Targeting
{
    [CreateAssetMenu(fileName = "BasicAttack", menuName = "Flow.Sample/Data/Targeting Data/Arc")]
    public class ArcTargetingData : CircleTargetingData
    {
        public float angle;
    }
}