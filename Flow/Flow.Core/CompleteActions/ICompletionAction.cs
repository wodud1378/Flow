using Flow.Core.Interfaces;

namespace Flow.Core.CompleteActions
{
    public interface ICompletionAction
    {
        public void Execute(IGameContext context);
    }
}