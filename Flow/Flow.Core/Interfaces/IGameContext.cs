using Flow.Core.Model;

namespace Flow.Core.Interfaces
{
    public interface IGameContext
    {
        public float TimeElapsed { get; set; }
        public IGameResult GetResult();
    }
}