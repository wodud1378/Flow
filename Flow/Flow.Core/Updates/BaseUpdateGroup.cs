using System.Collections.Generic;
using System.Linq;
using Flow.Core.Attributes;
using Flow.Core.Model;
using Flow.Core.Updates.Interfaces;

namespace Flow.Core.Updates
{
    public abstract class BaseUpdateGroup
    {
        public IReadOnlyList<IUpdate> Systems => _systems;
        
        public abstract UpdateType UpdateType { get; }
        
        public float DeltaTime { get; private set; }
        public float Time { get; private set; }
        
        private readonly List<IUpdate> _systems;

        protected BaseUpdateGroup(IEnumerable<IUpdate> systems)
        {
            _systems = systems
                .Where(s => s.IsManualUpdate(UpdateType))
                .ToSortedList();
            
            _systems.ForEach(x => x.Enabled = true);
        }

        public void Update(float deltaTime)
        {
            DeltaTime = deltaTime;
            Time += deltaTime;
            OnUpdate(deltaTime);
        }
        
        protected virtual void OnUpdate(float deltaTime)
        {
            _systems.ForEach(system => system.Update(deltaTime));
        }
    }
}