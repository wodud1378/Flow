using Flow.Core.Model;

namespace Flow.Core.Interruptions
{
    public abstract record InterruptAtGameState(GameState State) : IInterruptAt
    {
        public GameState State { get; } = State;
    }
}