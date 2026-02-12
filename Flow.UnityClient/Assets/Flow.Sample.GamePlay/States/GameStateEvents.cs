using R3;

namespace Flow.Sample.GamePlay.States
{
    public class GameStateEvents
    {
        public Observable<GameState> OnStateChanged => StateChangedStream;
        public Observable<Unit> OnGameStart => GameStartStream;
        public Observable<Unit> OnGamePause => GamePauseStream;
        public Observable<Unit> OnGameResume => GameResumeStream;
        public Observable<bool> OnGameEnd => GameEndStream; // true = victory, false = defeat

        internal Subject<GameState> StateChangedStream { get; } = new();
        internal Subject<Unit> GameStartStream { get; } = new();
        internal Subject<Unit> GamePauseStream { get; } = new();
        internal Subject<Unit> GameResumeStream { get; } = new();
        internal Subject<bool> GameEndStream { get; } = new();
    }
}
