using Flow.Core.Model;

namespace Flow.Sample.GamePlay.Models
{
    public readonly struct GameResult : IGameResult
    {
        public int Score => Metrics.Score;
        public bool Win { get; }
        
        public Metrics Metrics { get; }
        public Wave Wave { get; }
        
        public GameResult(Metrics metrics, Wave wave, bool win)
        {
            Metrics = metrics;
            Wave = wave;
            Win = win;
        }
    }
}