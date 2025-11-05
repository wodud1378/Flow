using System;
using System.Collections.Generic;
using Flow.Sample.GamePlay.Events;
using Flow.Sample.GamePlay.Models;
using Flow.Sample.GamePlay.Systems.Base;
using R3;
using VContainer;

namespace Flow.Sample.GamePlay.Systems
{
    public class UpdateContextSystem : BaseUpdateSystem
    {
        private readonly GameContext _context;
        private readonly IDisposable _disposable;
        private readonly Queue<Action> _updateActions = new();

        private Metrics? _pendingMetrics;
        private Wave? _pendingWave;

        [Inject]
        public UpdateContextSystem(GameContext context, EventChannels events)
        {
            _context = context;
            _disposable = Disposable.Combine(
                events.Game.OnTimeUpdated.Subscribe(OnTimeUpdated),
                events.Player.OnWaveUpdated.Subscribe(OnWaveUpdated),
                events.Player.OnMeticsUpdated.Subscribe(OnMeticsUpdated)
            );
        }

        ~UpdateContextSystem()
        {
            _disposable.Dispose();
        }

        #region Event Callbacks

        private void OnTimeUpdated(float deltaTime) => _updateActions.Enqueue(() => _context.TimeElapsed += deltaTime);

        private void OnWaveUpdated(Wave wave) => _updateActions.Enqueue(() => _context.Wave = wave);

        private void OnMeticsUpdated(Metrics metrics) => _updateActions.Enqueue(() => _context.Metrics = metrics);

        #endregion

        protected override void OnUpdate(float deltaTime)
        {
            while (_updateActions.Count > 0)
            {
                _updateActions.Dequeue()?.Invoke();
            }
        }
    }
}