using System.Collections.Generic;
using Flow.Core.CompleteActions;

namespace Flow.Core.Interfaces
{
    public interface ICompleteActionProvider
    {
        public (IReadOnlyList<ICompletionAction>, IReadOnlyList<IAsyncCompletionAction>) GetActions();
    }
}