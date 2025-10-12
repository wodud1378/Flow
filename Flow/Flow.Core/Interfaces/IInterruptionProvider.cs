using System.Collections.Generic;
using Flow.Core.Interruptions;

namespace Flow.Core.Interfaces
{
    public interface IInterruptionProvider
    {
        public List<IInterruption> ProvideInterruptions();
    }
}