using System.Collections.Generic;
using System.Linq;
using Flow.Core.Interfaces;
using Flow.Core.Updates;
using Flow.Core.Updates.Interfaces;
using VContainer;

namespace Flow.Sample.GamePlay.Services
{
    public class UpdateGroupProvider : IUpdateGroupProvider
    {
        private readonly List<BaseUpdateGroup> _updateGroups = new();
        
        [Inject]
        public UpdateGroupProvider(IEnumerable<IUpdate> updates)
        {
            var array = updates.ToArray();
            _updateGroups.Add(new UpdateGroup(array));
            _updateGroups.Add(new FixedUpdateGroup(array));
            _updateGroups.Add(new LateUpdateGroup(array));
        }

        public IReadOnlyList<BaseUpdateGroup> GetUpdateGroups() => _updateGroups;
    }
}