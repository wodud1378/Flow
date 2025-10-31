using Flow.Sample.Entities;
using Flow.Sample.Logic.Models;

namespace Flow.Sample.Logic
{
    public class DamageCalculator
    {
        public Damaged Calculate(BaseEntity attacker, BaseEntity target)
        {
            return new Damaged(attacker, target, new Damage(10));
        }
    }
}