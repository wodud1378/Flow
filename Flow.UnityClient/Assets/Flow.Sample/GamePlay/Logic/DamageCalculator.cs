using Flow.Sample.GamePlay.Components;
using Flow.Sample.Logic.Models;

namespace Flow.Sample.Logic
{
    public class DamageCalculator
    {
        public IDamage CalculateDamage(StatusComponent status)
        {
            var isCritical = UnityEngine.Random.Range(0f, 1f) < status.CriticalRate;
            var multiplier = isCritical ? status.CriticalMultiplier : 1f;
            var value = status.Damage * multiplier;
            
            return isCritical 
                ? new CriticalDamage(value)
                : new Damage(value);
        }
    }
}