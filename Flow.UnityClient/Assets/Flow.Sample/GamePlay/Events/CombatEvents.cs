using Flow.Sample.Logic.Models;
using R3;

namespace Flow.Sample.GamePlay.Events
{
    public class CombatEvents
    {
        public Observable<Damaged> OnDamaged => DamagedStream;
        
        internal Subject<Damaged> DamagedStream { get; } = new();
    }
}