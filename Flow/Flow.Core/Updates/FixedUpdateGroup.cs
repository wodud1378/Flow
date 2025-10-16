using System.Collections.Generic;
using Flow.Core.Model;
using Flow.Core.Updates.Interfaces;

namespace Flow.Core.Updates
{
    public class FixedUpdateGroup : BaseUpdateGroup
    {
        public override UpdateType UpdateType => UpdateType.FixedUpdate;
        
        public FixedUpdateGroup(IEnumerable<IUpdate> systems) : base(systems)
        {
        }
    }
}