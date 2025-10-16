using System.Collections.Generic;
using Flow.Core.Model;
using Flow.Core.Updates.Interfaces;

namespace Flow.Core.Updates
{
    public class LateUpdateGroup : BaseUpdateGroup
    {
        public override UpdateType UpdateType => UpdateType.LateUpdate;
        
        public LateUpdateGroup(IEnumerable<IUpdate> systems) : base(systems)
        {
        }
    }
}