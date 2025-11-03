using Flow.Sample.Entities;

namespace Flow.Sample.GamePlay.Systems.Models
{
    public readonly struct EnemyWave
    {
        public readonly EnemyEntity Prefab;
        public readonly int MonsterCount;
        public readonly float SpawnInterval;
        public readonly float TimeForNext;

        public EnemyWave(EnemyEntity prefab, int monsterCount, float spawnInterval, float timeForNext)
        {
            Prefab = prefab;
            MonsterCount = monsterCount;
            SpawnInterval = spawnInterval;
            TimeForNext = timeForNext;
        }
    }
}