using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Cysharp.Threading.Tasks;
using Flow.Core.CompleteActions;
using Flow.Core.Interfaces;
using Flow.Core.Interruptions;
using Flow.Core.Model;

namespace Flow.Core
{
    public class GameFlow
    {
        private readonly IGameContext _context;
        private readonly IGameStateHandler _gameStateHandler;
        
        private readonly List<IInterruption> _interruptions;
        private readonly IReadOnlyList<ICompletionAction> _completionActions;
        private readonly IReadOnlyList<IAsyncCompletionAction> _asyncCompletionActions;

        private CancellationToken _ct;
        private IInterruption _currentInterruption;

        public GameState State
        {
            get => _state;
            private set
            {
                if(_state == value) 
                    return;
                
                _state = value;
                _gameStateHandler?.OnGameState(_state);
            }
        }

        private GameState _state;

        public GameFlow(
            IGameContext context,
            IGameStateHandler gameStateHandler,
            IInterruptionProvider interruptionProvider,
            ICompleteActionProvider completeActionProvider)
        {
            _context = context;
            _gameStateHandler = gameStateHandler;
            _interruptions = interruptionProvider.ProvideInterruptions();

            var (actions, asyncActions) = completeActionProvider.GetActions();
            _completionActions = actions;
            _asyncCompletionActions = asyncActions;
        }
        
        public async UniTask RunAsync(CancellationToken ct)
        {
            _ct = ct;
            
            State = GameState.PreInitialization;
            await PlayInterruptionsAsync(GameState.PreInitialization);
            
            State = GameState.Initialization;
            await PlayInterruptionsAsync(GameState.Initialization);

            State = GameState.Running;
        }

        private void Update()
        {
            
        }
        
        private async UniTask PlayInterruptionsAsync(GameState state)
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

            while (enumerator.MoveNext() && !_ct.IsCancellationRequested)
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