using System;
using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;
using Flow.Sample.GamePlay.Components.Interfaces;
using Flow.Sample.GamePlay.Systems.Interfaces;
using UnityEngine;

namespace Flow.Sample.GamePlay.Systems.Base
{
    public abstract class BaseUpdateEntitySystem : BaseUpdateSystem
    {
        protected abstract Type[] EntityFilter { get; }
        
        private readonly IEntityContainer _entityContainer;
        private readonly IComponentProvider _componentCache;
        private readonly BaseEntity[] _entityBuffer;

        protected BaseUpdateEntitySystem(
            IEntityContainer entityContainer,
            IComponentProvider componentCache,
            int bufferSize)
        {
            _entityContainer = entityContainer;
            _componentCache = componentCache;
            _entityBuffer = new BaseEntity[bufferSize];
        }

        protected override void OnUpdate(float deltaTime)
        {
            int count = _entityContainer.GetEntities(_entityBuffer, EntityFilter);
            for (int i = 0; i < count; ++i)
            {
                OnUpdateEntity(_entityBuffer[i], i, deltaTime);
            }
        }
        
        protected abstract void OnUpdateEntity(BaseEntity entity, int index, float deltaTime);

        protected T As<T>(BaseEntity entity) where T : class, IComponent => 
            _componentCache.GetComponent<T>(entity);

        protected T As<T>(GameObject obj) where T : Component => 
            _componentCache.GetComponent<T>(obj);
    }
}