using System;

namespace Flow.Sample.GamePlay.Components.Interfaces
{
    public interface IAttackFxComponent
    {
        public event Action OnStart;
        public event Action OnDamage;
        public event Action OnEnd;
    }
}