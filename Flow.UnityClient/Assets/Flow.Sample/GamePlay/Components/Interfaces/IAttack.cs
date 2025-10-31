using Flow.Sample.Entities;
using Flow.Sample.GamePlay.Systems.Models;

namespace Flow.Sample.GamePlay.Components.Interfaces
{
    public interface IAttack : IComponent
    {
        public IDetectParams DetectParams { get; }
        public float Damage { get; }
        public float Critical { get; }
        
        public bool CanExecute(BaseEntity attacker, DetectScope<HealthComponent> scope);
        public void Execute(BaseEntity attacker, DetectScope<HealthComponent> scope);
    }
}