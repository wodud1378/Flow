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
        private readonly IMovePathProvider _pathProvider;
        private readonly EntitySystem _entitySystem;
        private readonly int _spawnLimitPerFrame;
        
        private readonly EnemyEvents _events;
        private readonly Queue<EnemyEntity> _request = new();
        
        private int _spawnCount;

        [Inject]
        public EnemySpawnSystem(
            IMovePathProvider pathProvider, 
            IConfig config, 
            EntitySystem entitySystem,
            EnemyEvents events)
        {
            _pathProvider = pathProvider;
            _entitySystem = entitySystem;
            _spawnLimitPerFrame = config.EnemySpawnLimitPerFrame;
            _events = events;
        }

        public void RequestSpawn(EnemyEntity prefab) => _request.Enqueue(prefab);

        protected override void OnUpdate(float deltaTime)
        {
            _spawnCount = 0;
            while (_request.Count > 0 && _spawnCount < _spawnLimitPerFrame)
            {
                var prefab = _request.Dequeue();
                var entity = _entitySystem.New(prefab);
                var move = entity.GetComponent<MoveOnPathComponent>();
                move.Initialize(_pathProvider.Provide());
                
                _events.EnemySpawnedStream.OnNext(entity);
                
                ++_spawnCount;
            }
        }
    }
}