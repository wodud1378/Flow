using Flow.Core.Model;

namespace Flow.Core.Interfaces
{
    public interface IGameStateHandler
    {
        public void OnGameState(GameState state);
    }
}