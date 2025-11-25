using Flow.Core.Interfaces;
using VContainer;

namespace Flow.Sample.GamePlay.Services
{
    public class FlowServiceProvider : IFlowServiceProvider
    {
        public IInterruptionProvider Interruption { get; }
        public ICompleteActionProvider CompleteAction { get; }
        public IUpdateGroupProvider UpdateGroup { get; }
        
        [Inject]
        public FlowServiceProvider(IInterruptionProvider interruption, ICompleteActionProvider completeAction, IUpdateGroupProvider updateGroup)
        {
            Interruption = interruption;
            CompleteAction = completeAction;
            UpdateGroup = updateGroup;
        }
    }
}