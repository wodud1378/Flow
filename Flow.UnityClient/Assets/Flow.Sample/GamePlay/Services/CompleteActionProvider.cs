using System.Collections.Generic;
using Flow.Core.CompleteActions;
using Flow.Core.Interfaces;

namespace Flow.Sample.GamePlay.Services
{
    public class CompleteActionProvider : ICompleteActionProvider
    {
        private readonly List<ICompletionAction> _actions = new();
        private readonly List<IAsyncCompletionAction> _asyncActions = new();
        
        public (IReadOnlyList<ICompletionAction>, IReadOnlyList<IAsyncCompletionAction>) GetActions() => (_actions, _asyncActions);
    }
}