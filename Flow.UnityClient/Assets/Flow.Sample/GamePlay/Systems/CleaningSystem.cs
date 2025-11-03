using System;
using System.Collections.Generic;
using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;
using Flow.Sample.GamePlay.Systems.Base;
using VContainer;
using Object = UnityEngine.Object;

namespace Flow.Sample.GamePlay.Systems
{
    public class CleaningSystem : BaseUpdateEntitySystem
    {
        protected override Type[] EntityFilter => null;

        private readonly Queue<BaseEntity> _queue = new();
        private readonly Queue<BaseEntity> _destroyQueue = new();
        private readonly IEntityContainer _entityContainer;
        private readonly PoolSystem _pool;
        private readonly ComponentCacheSystem _componentCache;

        [Inject]
        public CleaningSystem(IEntityContainer entityContainer, PoolSystem pool, ComponentCacheSystem componentCache) : base(
            entityContainer, componentCache, 256)
        {
            _entityContainer = entityContainer;
            _pool = pool;
            _componentCache = componentCache;
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            foreach (var entity in _queue)
            {
                _entityContainer.Unregister(entity);

                if (!entity.DestroyTriggered) 
                    continue;
                
                _componentCache.Clear(entity);
                _destroyQueue.Enqueue(entity);
                _pool.Destroy(entity);
            }
            
            DestroyLazy();
        }

        private void DestroyLazy()
        {
            if (!_destroyQueue.TryDequeue(out var entity))
                return;
            
            Object.Destroy(entity.gameObject);
        }

        protected override void OnUpdateEntity(BaseEntity entity, int index, float deltaTime)
        {
            if (entity.DestroyTriggered && entity.IsValid)
                entity.Invalidate();

            if (entity.IsValid)
                return;
            
            _queue.Enqueue(entity);
        }
    }
}