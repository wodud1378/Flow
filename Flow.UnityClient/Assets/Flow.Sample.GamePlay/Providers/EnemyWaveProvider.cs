using System.Collections.Generic;
using Flow.Sample.GamePlay.Entities;
using Flow.Sample.GamePlay.Systems.Interfaces;
using Flow.Sample.GamePlay.Systems.Models;
using UnityEngine;

namespace Flow.Sample.GamePlay.Providers
{
    public class EnemyWaveProvider : MonoBehaviour, IEnemyWaveProvider
    {
        [SerializeField] private List<WaveConfig> waves = new();

        private int _currentWaveIndex = -1;

        public bool IsLastWave() => _currentWaveIndex >= waves.Count - 1;

        public EnemyWave Next()
        {
            _currentWaveIndex++;
            if (_currentWaveIndex >= waves.Count)
            {
                _currentWaveIndex = waves.Count - 1;
            }

            var config = waves[_currentWaveIndex];
            return new EnemyWave(
                config.enemyPrefab,
                config.monsterCount,
                config.spawnInterval,
                config.timeForNext
            );
        }

        public void Reset()
        {
            _currentWaveIndex = -1;
        }

        [System.Serializable]
        public class WaveConfig
        {
            public EnemyEntity enemyPrefab;
            public int monsterCount = 10;
            public float spawnInterval = 1f;
            public float timeForNext = 5f;
        }
    }
}
