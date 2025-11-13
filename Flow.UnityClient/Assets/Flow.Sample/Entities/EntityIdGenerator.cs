using System;

namespace Flow.Sample.Entities
{
    public class EntityIdGenerator
    {
        private const int TowerIdStart = 1;
        private const int EnemyIdStart = 1001;
        private const int MiscIdStart = 5001;
        
        private readonly Type _towerType = typeof(TowerEntity);
        private readonly Type _enemyType = typeof(EnemyEntity);

        private int _currentTowerId = TowerIdStart;
        private int _currentEnemyId = EnemyIdStart;
        private int _currentMiscId = MiscIdStart;

        public int GenerateId<T>(T entity) where T : BaseEntity =>
            entity switch
            {
                TowerEntity tower => _currentTowerId,
                EnemyEntity enemy => _currentEnemyId,
                _ => _currentMiscId
            };
    }
}