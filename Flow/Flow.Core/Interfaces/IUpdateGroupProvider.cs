using System.Collections.Generic;
using Flow.Core.Updates;

namespace Flow.Core.Interfaces
{
    public interface IUpdateGroupProvider
    {
        public IReadOnlyList<BaseUpdateGroup> GetUpdateGroups();
    }
}