using System.Collections.Generic;
using Flow.Core.Model;
using Flow.Core.Updates.Interfaces;

namespace Flow.Core.Updates
{
    public class UpdateGroup : BaseUpdateGroup
    {
        public override UpdateType UpdateType => UpdateType.Update;
        
        public UpdateGroup(IEnumerable<IUpdate> systems) : base(systems)
        {
        }
    }
}