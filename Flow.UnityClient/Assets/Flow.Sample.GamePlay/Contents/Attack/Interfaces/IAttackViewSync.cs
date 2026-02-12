using System;
using Flow.Sample.GamePlay.Contents.Attack.Models;

namespace Flow.Sample.GamePlay.Contents.Attack.Interfaces
{
    public interface IAttackViewSync
    {
        public event Action<AttackContext, AttackViewEvent> OnViewEvent; 
        
        public void Play(AttackContext context);

        public void ManualUpdate(float deltaTime);
    }
}