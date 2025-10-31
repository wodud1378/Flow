using Flow.Sample.Entities;

namespace Flow.Sample.Logic.Models
{
    public interface IDamage
    {
        public float Value { get; }
    }

    public readonly struct Damage : IDamage
    {
        public float Value { get; }
        
        public Damage(float value)
        {
            Value = value;
        }
    }
    
    public readonly struct CriticalDamage : IDamage
    {
        public float Value { get; }
        
        public CriticalDamage(float value)
        {
            Value = value;
        }
    }

    public readonly struct Damaged
    {
        public readonly BaseEntity Attacker;
        public readonly BaseEntity Victim;
        public readonly IDamage Damage;

        public Damaged(BaseEntity attacker, BaseEntity victim, IDamage damage)
        {
            Attacker = attacker;
            Victim = victim;
            Damage = damage;
        }
    }
}