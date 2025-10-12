using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Flow.Core.Interruptions
{
    public class InterruptionFromCallback : IInterruption
    {
        public int Order { get; }
        public InterruptionState State { get; private set; }
        public IInterruptAt At { get; }
            
        private readonly Action<Action> _callback;
            
        public InterruptionFromCallback(int order, InterruptionState initialState, IInterruptAt at, Action<Action> callback)
        {
            Order = order;
            State = initialState;
            At = at;
            _callback = callback;
        }

        public async UniTask RunAsync(CancellationToken ct)
        {
            State = InterruptionState.Running;
            _callback.Invoke(()=> State = InterruptionState.Done);
            while (State != InterruptionState.Done && !ct.IsCancellationRequested)
            {
                await UniTask
                    .Yield(PlayerLoopTiming.Update, ct)
                    .SuppressCancellationThrow();
            }
        }
    }
}