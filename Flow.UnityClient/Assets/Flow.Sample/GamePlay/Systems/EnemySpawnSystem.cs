using System.Collections.Generic;
using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Configs;
using Flow.Sample.GamePlay.Events;
using Flow.Sample.GamePlay.Systems.Base;
using Flow.Sample.GamePlay.Systems.Interfaces;
using VContainer;

namespace Flow.Sample.GamePlay.Systems
{
    public class EnemySpawnSystem : BaseUpdateSystem
    {
        private readonly IEntityContainer _entityContainer;
        private readonly IMovePathProvider _pathProvider;
        private readonly int _spawnLimitPerFrame;
        
        private readonly EntityPoolSystem _pool;
        private readonly EnemyEvents _events;
        private readonly Queue<EnemyEntity> _request = new();
        
        private int _spawnCount;

        [Inject]
        public EnemySpawnSystem(
            IEntityContainer entityContainer, 
            IMovePathProvider pathProvider, 
            IConfig config, 
            EntityPoolSystem pool,
            EnemyEvents events)
        {
            _entityContainer = entityContainer;
            _pathProvider = pathProvider;
            _spawnLimitPerFrame = config.EnemySpawnLimitPerFrame;
            _pool = pool;
            _events = events;
        }

        public void RequestSpawn(EnemyEntity prefab) => _request.Enqueue(prefab);

        protected override void OnUpdate(float deltaTime)
        {
            _spawnCount = 0;
            while (_request.Count > 0 && _spawnCount < _spawnLimitPerFrame)
            {
                var prefab = _request.Dequeue();
                var entity = _pool.GetObject(prefab);
                var move = entity.GetComponent<MoveOnPathComponent>();
                move.Initialize(_pathProvider.Provide());
                
                _entityContainer.Register(entity);
                _events.EnemySpawnedStream.OnNext(entity);
                
                ++_spawnCount;
            }
        }
    }
}