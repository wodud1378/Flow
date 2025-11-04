using System;

namespace Flow.Sample.GamePlay.Contents.Attack.Interfaces
{
    public interface IAttackViewSync
    {
        public event Action OnHitTiming;
        public event Action OnEnd;
        
        public void Play();
    }
}