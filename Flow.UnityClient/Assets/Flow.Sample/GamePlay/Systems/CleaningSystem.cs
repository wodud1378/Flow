using System;
using System.Collections.Generic;
using Flow.Sample.GamePlay.Entities;
using Flow.Sample.GamePlay.Entities.Interfaces;
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
        private readonly EntitySystem _entitySystem;

        [Inject]
        public CleaningSystem(EntitySystem entitySystem, IEntityContainer entityContainer)
            : base(entityContainer, 256)
        {
            _entitySystem = entitySystem;
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            foreach (var entity in _queue)
            {
                if (!entity.DestroyTriggered)
                    continue;

                _destroyQueue.Enqueue(entity);
            }

            DestroyLazy();
        }

        private void DestroyLazy()
        {
            if (!_destroyQueue.TryDequeue(out var entity))
                return;

            if (!entity.DestroyTriggered)
                return;

            Object.Destroy(entity.gameObject);
        }

        protected override void OnUpdateEntity(BaseEntity entity, int index, float deltaTime)
        {
            if (entity.DestroyTriggered && entity.IsValid)
                _entitySystem.Invalidate(entity);

            if (entity.IsValid)
                return;

            _queue.Enqueue(entity);
        }
    }
}
