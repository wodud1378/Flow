using Flow.Sample.GamePlay.Models;
using Flow.Sample.GamePlay.Systems.Base;
using VContainer;

namespace Flow.Sample.GamePlay.Systems
{
    public class ContextUpdateSystem : BaseUpdateSystem
    {
        private readonly GameContext _context;

        private Metrics? _pendingMetrics;
        
        [Inject]
        public ContextUpdateSystem(GameContext context)
        {
            _context = context;
        }
        
        protected override void OnUpdate(float deltaTime)
        {
            if (!_pendingMetrics.HasValue)
                return;

            _context.Metrics = _pendingMetrics.Value;
        }
    }
}