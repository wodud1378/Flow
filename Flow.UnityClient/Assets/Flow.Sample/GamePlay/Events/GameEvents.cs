using Flow.Sample.Entities;
using Flow.Sample.GamePlay.Models;
using Flow.Sample.Logic.Models;
using R3;

namespace Flow.Sample.GamePlay.Events
{
    public class GameEvents
    {
        public Observable<float> OnTimeUpdated => TimeUpdatedStream;
        public Observable<Wave> OnWaveUpdated => WaveUpdateStream;
        public Observable<Metrics> OnMeticsUpdated => MeticsUpdatedStream;
        public Observable<Damaged> OnDamaged => DamagedStream;
        public Observable<EnemyEntity> OnEnemyKilled => EnemyKilledStream;
        
        internal Subject<float> TimeUpdatedStream { get; } = new();
        internal Subject<Wave> WaveUpdateStream { get; } = new();
        internal Subject<Metrics> MeticsUpdatedStream { get; } = new();
        internal Subject<Damaged> DamagedStream { get; } = new();
        internal Subject<EnemyEntity> EnemyKilledStream { get; } = new();
    }
}