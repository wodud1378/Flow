using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;
using Flow.Sample.GamePlay.Systems.Base;
using Flow.Sample.GamePlay.Systems.Interfaces;
using Flow.Sample.GamePlay.Systems.Models;
using VContainer;

namespace Flow.Sample.GamePlay.Systems
{
    public class WaveSystem : BaseUpdateSystem
    {
        private readonly IEnemyWaveProvider _waveProvider;
        private readonly IEntityContainer _entityContainer;
        private readonly UpdateContextSystem _updateContext;
        private readonly EntityPoolSystem _entityPool;

        private EnemyWave _currentWave;
        private int _spawnedCount;
        private float _spawnIntervalElpased;
        private float _timeForNextElapsed;

        [Inject]
        public WaveSystem(IEnemyWaveProvider waveProvider,
            IEntityContainer entityContainer,
            UpdateContextSystem updateContext,
            EntityPoolSystem entityPool)
        {
            _waveProvider = waveProvider;
            _entityContainer = entityContainer;
            _updateContext = updateContext;
            _entityPool = entityPool;
        }

        protected override void OnUpdate(float deltaTime)
        {
        }

        private void ChangeWave(EnemyWave wave)
        {
            _currentWave = wave;
            _spawnedCount = 0;
            _spawnIntervalElpased = 0f;
            _timeForNextElapsed = 0f;
        }

        private void UpdateWave(float deltaTime)
        {
            if (_spawnedCount < _currentWave.MonsterCount)
            {
            }
            else
            {
                OnAllEnemySpawned(deltaTime);
            }
        }

        private void UpdateSpawn(float deltaTime)
        {
            _spawnIntervalElpased += deltaTime;
            if (_spawnIntervalElpased < _currentWave.SpawnInterval)
                return;

            var enemy = _entityPool.GetObject(_currentWave.Prefab);
            _entityContainer.Register(enemy);
        }

        private void OnAllEnemySpawned(float deltaTime)
        {
            _timeForNextElapsed += deltaTime;
            if (_timeForNextElapsed < _currentWave.TimeForNext)
                return;

            if (_waveProvider.IsLastWave())
                return;
            
            ChangeWave(_waveProvider.Next());
        }
    }
}