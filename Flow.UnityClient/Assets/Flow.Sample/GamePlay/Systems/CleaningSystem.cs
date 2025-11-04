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
        private readonly EntityPoolSystem _entityPool;
        private readonly ComponentCacheSystem _componentCache;

        [Inject]
        public CleaningSystem(IEntityContainer entityContainer, EntityPoolSystem entityPool, ComponentCacheSystem componentCache) : base(
            entityContainer, componentCache, 256)
        {
            _entityContainer = entityContainer;
            _entityPool = entityPool;
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
                _entityPool.Destroy(entity);
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