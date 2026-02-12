using System;
using Flow.Sample.GamePlay.Entities.Interfaces;
using Flow.Sample.GamePlay.Events;
using Flow.Sample.GamePlay.Systems.Base;
using R3;
using UnityEngine;
using VContainer;

namespace Flow.Sample.GamePlay.States
{
    public class GameStateSystem : BaseUpdateSystem
    {
        private readonly GameStateEvents _stateEvents;
        private readonly PlayerEvents _playerEvents;
        private readonly EnemyEvents _enemyEvents;
        private readonly IEntityContainer _entityContainer;
        private readonly IDisposable _subscriptions;

        private GameState _currentState = GameState.None;
        private bool _allWavesComplete;
        private int _remainingEnemies;

        public GameState CurrentState => _currentState;

        [Inject]
        public GameStateSystem(
            GameStateEvents stateEvents,
            PlayerEvents playerEvents,
            EnemyEvents enemyEvents,
            IEntityContainer entityContainer)
        {
            _stateEvents = stateEvents;
            _playerEvents = playerEvents;
            _enemyEvents = enemyEvents;
            _entityContainer = entityContainer;

            _subscriptions = Disposable.Combine(
                playerEvents.OnPlayerDead.Subscribe(_ => OnPlayerDead()),
                enemyEvents.OnEnemySpawned.Subscribe(_ => _remainingEnemies++),
                enemyEvents.OnEnemyKilled.Subscribe(_ => OnEnemyKilled())
            );
        }

        ~GameStateSystem()
        {
            _subscriptions?.Dispose();
        }

        protected override void OnStartRunning()
        {
            ChangeState(GameState.Ready);
        }

        public void StartGame()
        {
            if (_currentState != GameState.Ready)
                return;

            ChangeState(GameState.Playing);
            _stateEvents.GameStartStream.OnNext(Unit.Default);
        }

        public void PauseGame()
        {
            if (_currentState != GameState.Playing)
                return;

            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            _stateEvents.GamePauseStream.OnNext(Unit.Default);
        }

        public void ResumeGame()
        {
            if (_currentState != GameState.Paused)
                return;

            ChangeState(GameState.Playing);
            Time.timeScale = 1f;
            _stateEvents.GameResumeStream.OnNext(Unit.Default);
        }

        public void SetAllWavesComplete()
        {
            _allWavesComplete = true;
            CheckVictoryCondition();
        }

        private void OnPlayerDead()
        {
            if (_currentState == GameState.GameOver || _currentState == GameState.Victory)
                return;

            ChangeState(GameState.GameOver);
            Time.timeScale = 0f;
            _stateEvents.GameEndStream.OnNext(false);
        }

        private void OnEnemyKilled()
        {
            _remainingEnemies--;
            CheckVictoryCondition();
        }

        private void CheckVictoryCondition()
        {
            if (!_allWavesComplete || _remainingEnemies > 0)
                return;

            if (_currentState == GameState.GameOver || _currentState == GameState.Victory)
                return;

            ChangeState(GameState.Victory);
            _stateEvents.GameEndStream.OnNext(true);
        }

        private void ChangeState(GameState newState)
        {
            if (_currentState == newState)
                return;

            _currentState = newState;
            _stateEvents.StateChangedStream.OnNext(newState);
        }

        protected override void OnUpdate(float deltaTime)
        {
            // Handle pause input
            if (UnityEngine.Input.GetKeyDown(KeyCode.P))
            {
                if (_currentState == GameState.Playing)
                    PauseGame();
                else if (_currentState == GameState.Paused)
                    ResumeGame();
            }

            // Handle space to start
            if (_currentState == GameState.Ready && UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }
        }
    }
}
