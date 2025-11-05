using R3;
using VContainer;

namespace Flow.Sample.GamePlay.Events
{
    public class GameEvents
    {
        public Observable<float> OnTimeUpdated => TimeUpdatedStream;
        
        internal Subject<float> TimeUpdatedStream { get; } = new();
    }
}