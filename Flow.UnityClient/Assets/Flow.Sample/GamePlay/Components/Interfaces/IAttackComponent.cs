using Flow.Sample.GamePlay.Systems;
using Flow.Sample.GamePlay.Systems.Models;

namespace Flow.Sample.GamePlay.Components.Interfaces
{
    public interface IAttackComponent : IComponent
    {
        public IDetectParams DetectParams { get; }
        
        public bool TryExecute(DetectScope<StatusComponent> scope);
        public void ApplyDamage(DetectScope<StatusComponent> scope, CombatSystem combatSystem);
    }
}