using Flow.Core.Interfaces;
using Flow.Core.Model;
using Flow.Sample.GamePlay.Models;

namespace Flow.Sample.GamePlay
{
    public class GameContext : IGameContext
    {
        public Metrics Metrics { get; set; }
        public Wave Wave { get; set; }
        public bool Win { get; set; }
        
        public float TimeElapsed { get; set; }

        public IGameResult GetResult() => new GameResult(Metrics, Wave, Win);
    }
}