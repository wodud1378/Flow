using Flow.Sample.GamePlay.Models;
using R3;

namespace Flow.Sample.GamePlay.Events
{
    public class PlayerEvents
    {
        public Observable<Unit> OnPlayerDead => PlayerDeadStream;
        public Observable<float> OnHpChanged => HpChangedStream;
        public Observable<Wave> OnWaveUpdated => WaveUpdateStream;
        public Observable<Metrics> OnMeticsUpdated => MeticsUpdatedStream;
        
        internal Subject<Unit> PlayerDeadStream { get; } = new();
        internal Subject<float> HpChangedStream { get; } = new();
        internal Subject<Wave> WaveUpdateStream { get; } = new();
        internal Subject<Metrics> MeticsUpdatedStream { get; } = new();
    }
}