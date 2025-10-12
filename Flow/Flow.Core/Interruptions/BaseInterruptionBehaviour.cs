using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Flow.Core.Interruptions
{
    public abstract class BaseInterruptionBehaviour : MonoBehaviour, IInterruption
    {
        public abstract int Order { get; }
        public abstract IInterruptAt At { get; }

        public virtual InterruptionState State { get; protected set; } = InterruptionState.Undefined;
        
        public async UniTask RunAsync(CancellationToken ct)
        {
            if (State != InterruptionState.RequireRun)
                return;

            State = InterruptionState.Running;

            await RunAsyncInternal(ct)
                .SuppressCancellationThrow();

            State = InterruptionState.Done;
        }
        
        protected abstract UniTask RunAsyncInternal(CancellationToken ct);
    }
}