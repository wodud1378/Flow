using Flow.Sample.Entities.Interfaces;
using Flow.Sample.GamePlay.Events;
using Flow.Sample.GamePlay.Models;
using Flow.Sample.GamePlay.Systems.Base;
using Flow.Sample.GamePlay.Systems.Interfaces;
using Flow.Sample.GamePlay.Systems.Models;
using VContainer;

namespace Flow.Sample.GamePlay.Systems
{
    public class WaveSystem : BaseUpdateSystem
    {
        private readonly IEnemyWaveProvider _waveProvider;
        private readonly EnemySpawnSystem _spawnSystem;
        private readonly PlayerEvents _events;

        private Wave _wave;
        
        private EnemyWave _enemyWave;
        private int _spawnedCount;
        private float _spawnIntervalElapsed;
        private float _timeForNextElapsed;

        [Inject]
        public WaveSystem(IEnemyWaveProvider waveProvider,
            IEntityContainer entityContainer,
            EnemySpawnSystem spawnSystem,
            PlayerEvents events)
        {
            _wave = new Wave(0, 0, 0);
            
            _waveProvider = waveProvider;
            _events = events;
            _spawnSystem = spawnSystem;
        }

        protected override void OnStartRunning()
        {
            ChangeWave(_waveProvider.Next());
        }

        protected override void OnUpdate(float deltaTime)
        {
            UpdateWave(deltaTime);
        }

        private void ChangeWave(EnemyWave wave)
        {
            ++_wave.Number;
            _wave.EnemyKilled = 0;
            _wave.TimeSinceStart = 0;
            _events.WaveUpdateStream.OnNext(_wave);
            
            _enemyWave = wave;
            _spawnedCount = 0;
            _spawnIntervalElapsed = 0f;
            _timeForNextElapsed = 0f;
        }

        private void UpdateWave(float deltaTime)
        {
            if (_spawnedCount < _enemyWave.MonsterCount)
            {
                UpdateSpawn(deltaTime);
            }
            else
            {
                OnAllEnemySpawned(deltaTime);
            }
        }

        private void UpdateSpawn(float deltaTime)
        {
            _spawnIntervalElapsed += deltaTime;
            if (_spawnIntervalElapsed < _enemyWave.SpawnInterval)
                return;

            _spawnSystem.RequestSpawn(_enemyWave.Prefab);
            ++_spawnedCount;
        }

        private void OnAllEnemySpawned(float deltaTime)
        {
            _timeForNextElapsed += deltaTime;
            if (_timeForNextElapsed < _enemyWave.TimeForNext)
                return;

            if (_waveProvider.IsLastWave())
            {
                Enabled = false;
                return;
            }
            
            ChangeWave(_waveProvider.Next());
        }
    }
}