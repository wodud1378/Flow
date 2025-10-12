using Cysharp.Threading.Tasks;
using Flow.Core.Interfaces;

namespace Flow.Core.CompleteActions
{
    public interface IAsyncCompletionAction
    {
        public UniTask ExecuteAsync(IGameContext context);
    }
}