using System.Collections.Generic;
using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Systems.Base;
using UnityEngine;

namespace Flow.Sample.GamePlay.Systems
{
    public class WaveSystem : BaseSystem
    {
        private readonly IEntityContainer _entityContainer;
        private readonly WaveData[] _waves;
        private readonly Transform _spawnPoint;
        private readonly Vector3[] _enemyPath;
        
        private int _currentWaveIndex = 0;
        private float _waveSpawnTimer = 0f;
        private int _enemiesSpawnedInWave = 0;
        private float _timeBetweenEnemies = 1f;
        private bool _waveInProgress = false;
        private int _enemiesAlive = 0;
        
        public int CurrentWave => _currentWaveIndex + 1;
        public bool IsWaveComplete => !_waveInProgress && _enemiesAlive == 0;
        public bool AllWavesComplete => _currentWaveIndex >= _waves.Length;
        
        public WaveSystem(IEntityContainer entityContainer, WaveData[] waves, Transform spawnPoint, Vector3[] path)
        {
            this._entityContainer = entityContainer;
            this._waves = waves;
            this._spawnPoint = spawnPoint;
            this._enemyPath = path;
        }
        
        protected override void OnUpdate(float deltaTime)
        {
            if (AllWavesComplete) return;
            
            // Check if should start next wave
            if (!_waveInProgress && IsWaveComplete && _currentWaveIndex < _waves.Length)
            {
                StartWave();
            }
            
            // Spawn enemies in current wave
            if (_waveInProgress)
            {
                _waveSpawnTimer += deltaTime;
                
                if (_waveSpawnTimer >= _timeBetweenEnemies)
                {
                    SpawnNextEnemy();
                    _waveSpawnTimer = 0f;
                }
            }
            
            // Count alive enemies
            UpdateEnemyCount();
        }
        
        public void StartWave()
        {
            if (_currentWaveIndex >= _waves.Length) return;
            
            var wave = _waves[_currentWaveIndex];
            _waveInProgress = true;
            _enemiesSpawnedInWave = 0;
            _timeBetweenEnemies = wave.spawnInterval;
            _waveSpawnTimer = _timeBetweenEnemies; // Spawn first enemy immediately
        }
        
        private void SpawnNextEnemy()
        {
            if (_currentWaveIndex >= _waves.Length) return;
            
            var wave = _waves[_currentWaveIndex];
            if (_enemiesSpawnedInWave >= wave.enemyCount)
            {
                _waveInProgress = false;
                _currentWaveIndex++;
                return;
            }
            
            var enemy = SpawnEnemy(wave.enemyPrefab);
            if (enemy != null)
            {
                _enemiesSpawnedInWave++;
                _enemiesAlive++;
            }
        }
        
        private GameObject SpawnEnemy(GameObject prefab)
        {
            if (prefab == null || _spawnPoint == null) return null;
            
            var enemyGo = Object.Instantiate(prefab, _spawnPoint.position, _spawnPoint.rotation);
            var enemyEntity = enemyGo.GetComponent<EnemyEntity>();
            
            if (enemyEntity != null)
            {
                enemyEntity.Initialize(GenerateEntityId());
                enemyEntity.SetPath(_enemyPath);
                _entityContainer.Register(enemyEntity);
            }
            
            return enemyGo;
        }
        
        private void UpdateEnemyCount()
        {
            _enemiesAlive = 0;
            var enemies = _entityContainer.GetEntities<EnemyEntity>();
            foreach (var enemy in enemies)
            {
                if (enemy.IsValid)
                {
                    _enemiesAlive++;
                }
            }
        }
        
        private int GenerateEntityId()
        {
            // Simple ID generation - should use proper ID system in production
            return Random.Range(10000, 99999);
        }
        
        public void SkipToNextWave()
        {
            if (_currentWaveIndex < _waves.Length - 1)
            {
                _currentWaveIndex++;
                _waveInProgress = false;
                _enemiesSpawnedInWave = 0;
            }
        }
    }
    
    [System.Serializable]
    public class WaveData
    {
        public string name = "Wave";
        public GameObject enemyPrefab;
        public int enemyCount = 10;
        public float spawnInterval = 1f;
        public float waveDelay = 5f;
    }
}
