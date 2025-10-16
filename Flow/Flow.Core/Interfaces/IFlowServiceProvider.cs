namespace Flow.Core.Interfaces
{
    public interface IFlowServiceProvider
    {
        public IInterruptionProvider Interruption { get; }
        public ICompleteActionProvider CompleteAction { get; }
        public IUpdateGroupProvider UpdateGroup { get; }
    }
}