using System.Threading;
using Cysharp.Threading.Tasks;

namespace Flow.Core.Interruptions
{
    public enum InterruptionState
    {
        Undefined,
        RequireRun,
        Running,
        Done,
    }
    
    public interface IInterruption
    {
        public int Order { get; }
        public InterruptionState State { get; }
        public IInterruptAt At { get; }

        public UniTask RunAsync(CancellationToken ct);
    }
}