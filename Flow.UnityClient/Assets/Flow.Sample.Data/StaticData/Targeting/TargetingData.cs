using UnityEngine;

namespace Flow.Sample.Data.StaticData.Targeting
{
    public abstract class TargetingData : ScriptableObject
    {
        public int maxTarget;
        public LayerMask targetLayers;
    }
}