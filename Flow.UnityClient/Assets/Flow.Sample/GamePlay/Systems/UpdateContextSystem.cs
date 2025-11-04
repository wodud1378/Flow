using Flow.Sample.GamePlay.Events;
using Flow.Sample.GamePlay.Models;
using Flow.Sample.GamePlay.Systems.Base;
using VContainer;

namespace Flow.Sample.GamePlay.Systems
{
    public class UpdateContextSystem : BaseUpdateSystem
    {
        private readonly GameContext _context;
        private readonly GameEvents _events;

        private Metrics? _pendingMetrics;
        private Wave? _pendingWave;
        
        [Inject]
        public UpdateContextSystem(GameContext context, GameEvents events)
        {
            _context = context;
            _events = events;
        }
        
        protected override void OnUpdate(float deltaTime)
        {
            UpdateContext(deltaTime);
        }

        private void UpdateContext(float deltaTime)
        {
            if (_pendingMetrics.HasValue)
            {
                _context.Metrics = _pendingMetrics.Value;
                _events.MeticsUpdatedStream.OnNext(_context.Metrics);
                
                _pendingMetrics = null;
            }

            if (_pendingWave.HasValue)
            {
                _context.Wave = _pendingWave.Value;
                _events.WaveChangedStream.OnNext(_context.Wave);
                
                _pendingWave = null;
            }
            
            _context.TimeElapsed += deltaTime;
            _events.TimeUpdatedStream.OnNext(_context.TimeElapsed);
        }
    }
}