using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Flow.Core.CompleteActions;
using Flow.Core.Interfaces;
using Flow.Core.Interruptions;
using Flow.Core.Model;
using Flow.Core.Updates;
using UnityEngine;

namespace Flow.Core
{
    public class GameFlow : MonoBehaviour
    {
        private readonly List<IGameStateHandler> _gameStateHandlers = new();
            
        private IGameContext _context;
        
        private List<IInterruption> _interruptions;
        private IReadOnlyList<ICompletionAction> _completionActions;
        private IReadOnlyList<IAsyncCompletionAction> _asyncCompletionActions;

        private Dictionary<UpdateType, BaseUpdateGroup> _updateGroups;

        private CancellationToken _ct;
        private CancellationTokenSource _runningStateCts;
        private CancellationTokenSource _linkedCts;
        
        private IInterruption _currentInterruption;

        public GameState State
        {
            get => _state;
            private set
            {
                if(_state == value) 
                    return;
                
                _state = value;
                _gameStateHandlers.ForEach(handler => handler.OnGameState(_state));
            }
        }

        private GameState _state;

        public void InitializeDependencies(
            IGameContext context,
            IFlowServiceProvider serviceProvider)
        {
            _context = context;
            _interruptions = serviceProvider.Interruption.ProvideInterruptions();

            var (actionList, asyncActionList) = serviceProvider.CompleteAction.GetActions();
            _completionActions = actionList;
            _asyncCompletionActions = asyncActionList;

            _updateGroups = serviceProvider.UpdateGroup
                .GetUpdateGroups()
                .ToDictionary(x => x.UpdateType, x => x);
        }
        
        public void RegisterGameStateHandler(IGameStateHandler handler) => _gameStateHandlers.Add(handler);
        
        public async UniTask RunAsync()
        {
            _ct = this.GetCancellationTokenOnDestroy();
            
            State = GameState.Initialization;
            await PlayInterruptionsAsync(GameState.Initialization, _ct);

            _runningStateCts = new CancellationTokenSource();
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_ct, _runningStateCts.Token);
            State = GameState.Running;
        }

        public async UniTask QuitAsync()
        {
            _runningStateCts?.Cancel();
            State = GameState.End;

            foreach (var action in _completionActions)
            {
                action.Execute(_context);
            }

            foreach (var action in _asyncCompletionActions)
            {
                await action.ExecuteAsync(_context);
            }
        }

        private void Update()
        {
            if (!EnableUpdate())
                return;

            var deltaTime = Time.deltaTime;
            if (_updateGroups.TryGetValue(UpdateType.Update, out var group))
                group.Update(deltaTime);

            MonitorRunningInterruptions().Forget();
        }

        private void FixedUpdate()
        {
            if (!EnableUpdate())
                return;

            if (_updateGroups.TryGetValue(UpdateType.FixedUpdate, out var group))
                group.Update(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            if (!EnableUpdate())
                return;
            
            if (_updateGroups.TryGetValue(UpdateType.LateUpdate, out var group))
                group.Update(Time.deltaTime);
        }

        private bool EnableUpdate() => State == GameState.Running && _currentInterruption == null;
        
        private async UniTask MonitorRunningInterruptions()
        {
            while (State == GameState.Running && !_linkedCts.IsCancellationRequested)
            {
                await PlayInterruptionsAsync(GameState.Running, _linkedCts.Token);
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }
        
        private async UniTask PlayInterruptionsAsync(GameState state, CancellationToken ct)
        {
            if (_currentInterruption != null)
                return;

            if (_interruptions.Count == 0)
                return;

            using var enumerator = _interruptions
                .Where(x =>
                    x.State == InterruptionState.RequireRun &&
                    x.At is InterruptAtGameState s &&
                    s.State == state
                )
                .OrderBy(x => x.Order)
                .GetEnumerator();

            while (enumerator.MoveNext() && !ct.IsCancellationRequested)
            {
                var interruption = enumerator.Current;
                if (interruption == null)
                {
                    _currentInterruption = null;
                    continue;
                }
                
                _currentInterruption = interruption;
                await interruption.RunAsync(_ct);
                
                _currentInterruption = null;
            }
        }
    }
}