using Flow.Sample.GamePlay.Models;
using Flow.Sample.Logic.Models;
using R3;

namespace Flow.Sample.GamePlay.Events
{
    public class GameEvents
    {
        public Observable<float> OnTimeUpdated => TimeUpdatedStream;
        public Observable<Wave> OnWaveChanged => WaveChangedStream;
        public Observable<Metrics> OnMeticsUpdated => MeticsUpdatedStream;
        public Observable<Damaged> OnDamaged => DamagedStream;
        
        internal Subject<float> TimeUpdatedStream { get; } = new();
        internal Subject<Wave> WaveChangedStream { get; } = new();
        internal Subject<Metrics> MeticsUpdatedStream { get; } = new();
        internal Subject<Damaged> DamagedStream { get; } = new();
    }
}