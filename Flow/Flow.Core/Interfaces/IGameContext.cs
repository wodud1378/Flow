using Flow.Core.Model;

namespace Flow.Core.Interfaces
{
    public interface IGameContext
    {
        public float TimeElapsed { get; }
        public IGameResult GetResult();
    }
}