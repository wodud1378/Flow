using Flow.Sample.GamePlay.Contents.Attack.Interfaces;
using UnityEngine;

namespace Flow.Sample.GamePlay.Contents.Attack.Delay
{
    public class CoolDown : IAttackCondition
    {
        public bool Ready => LeftTime <= 0f;

        public float Duration
        {
            get => _duration;
            set => _duration = Mathf.Max(0f, value);
        }

        public float LeftTime
        {
            get => _leftTime;
            set => _leftTime = Mathf.Max(0f, Duration, value);
        }
        
        private float _duration;
        private float _leftTime;

        public CoolDown(float duration)
        {
            Duration = duration;
            LeftTime = 0f;
        }

        public void Reset() => LeftTime = Duration;

        public void Update(float deltaTime)
        {
            if (Ready)
                return;

            LeftTime -= deltaTime;
        }
    }
}