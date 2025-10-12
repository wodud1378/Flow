using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Flow.Core.Interruptions
{
    public class Interruption : IInterruption
    {
        public int Order { get; }
        public InterruptionState State { get; private set; }
        public IInterruptAt At { get; }

        private readonly Func<UniTask> _getTask;
        
        public async UniTask RunAsync(CancellationToken ct)
        {
            if (State != InterruptionState.RequireRun)
                return;
            
            State = InterruptionState.Running;
            
            await _getTask
                .Invoke()
                .AttachExternalCancellation(ct)
                .SuppressCancellationThrow();

            State = InterruptionState.Done;
        }

        public Interruption(IInterruptAt at, Func< UniTask> task, int order = -1, InterruptionState state = InterruptionState.RequireRun)
        {
            _getTask = task;
            At = at;
            State = state;
            Order = order;
        }
    }
}