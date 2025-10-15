using UnityEngine;

namespace Flow.Sample.GamePlay.Components
{
    public class SpeedComponent : MonoBehaviour, IComponent
    {
        [SerializeField] private float baseSpeed = 5f;
        [SerializeField] private float speedMultiplier = 1f;
        
        public float BaseSpeed
        {
            get => baseSpeed;
            set => baseSpeed = value;
        }
        
        public float SpeedMultiplier
        {
            get => speedMultiplier;
            set => speedMultiplier = Mathf.Max(0.1f, value);
        }
        
        public float CurrentSpeed => baseSpeed * speedMultiplier;
        
        public void ApplySlow(float slowPercent)
        {
            speedMultiplier = Mathf.Clamp01(1f - slowPercent);
        }
        
        public void ResetSpeed()
        {
            speedMultiplier = 1f;
        }
    }
}
