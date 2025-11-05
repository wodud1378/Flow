using Flow.Sample.Entities;
using R3;

namespace Flow.Sample.GamePlay.Events
{
    public class EnemyEvents
    {
        public Observable<EnemyEntity> OnEnemySpawned => EnemySpawnedStream;
        public Observable<EnemyEntity> OnEnemyKilled => EnemyKilledStream;
        
        internal Subject<EnemyEntity> EnemySpawnedStream { get; } = new();
        internal Subject<EnemyEntity> EnemyKilledStream { get; } = new();
    }
}