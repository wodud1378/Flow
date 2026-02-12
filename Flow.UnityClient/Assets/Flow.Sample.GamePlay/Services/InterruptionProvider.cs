using System;
using System.Collections.Generic;
using Flow.Core.Interfaces;
using Flow.Core.Interruptions;

namespace Flow.Sample.GamePlay.Services
{
    [Serializable]
    public class InterruptionProvider : IInterruptionProvider
    {
        // TODO : 인스펙터에서 연출 구현 및 등록
        private readonly List<IInterruption> _interruptions = new();

        public List<IInterruption> ProvideInterruptions() => _interruptions;
    }
}