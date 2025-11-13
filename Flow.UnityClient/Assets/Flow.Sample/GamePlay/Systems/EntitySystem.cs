using System;
using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;
using VContainer;

namespace Flow.Sample.GamePlay.Systems
{
    public class EntitySystem
    {
        private readonly IEntityContainer _entityContainer;
        private readonly PoolSystem _poolSystem;
        private readonly ComponentCacheSystem _cacheSystem;
        private readonly EntityIdGenerator _idGenerator;

        [Inject]
        public EntitySystem(IEntityContainer entityContainer, PoolSystem poolSystem, ComponentCacheSystem cacheSystem, EntityIdGenerator idGenerator)
        {
            _entityContainer = entityContainer;
            _poolSystem = poolSystem;
            _cacheSystem = cacheSystem;
            _idGenerator = idGenerator;
        }

        public T New<T>(T prefab, Action<T> onBeforeActive = null) where T : BaseEntity
        {
            var entity = _poolSystem.GetObject(prefab, onBeforeActive);
            if(entity.DestroyTriggered)
                entity.CancelDestroySelf();

            var id = _idGenerator.GenerateId(entity);
            entity.Initialize(id, _cacheSystem);
            _entityContainer.Register(entity);

            return entity;
        }
        
        public void Invalidate<T>(T entity) where T : BaseEntity
        {
            entity.Invalidate();
            
            _poolSystem.Destroy(entity);
            _entityContainer.Unregister(entity);
        }
    }
}