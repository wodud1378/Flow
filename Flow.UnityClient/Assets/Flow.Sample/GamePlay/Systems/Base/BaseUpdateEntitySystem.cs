using System;
using Flow.Sample.GamePlay.Components.Interfaces;
using Flow.Sample.GamePlay.Entities;
using Flow.Sample.GamePlay.Entities.Interfaces;

namespace Flow.Sample.GamePlay.Systems.Base
{
    public abstract class BaseUpdateEntitySystem : BaseUpdateSystem
    {
        protected abstract Type[] EntityFilter { get; }

        private readonly IEntityContainer _entityContainer;
        private readonly BaseEntity[] _entityBuffer;

        protected BaseUpdateEntitySystem(IEntityContainer entityContainer, int bufferSize)
        {
            _entityContainer = entityContainer;
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
            entity.GetSystemComponent<T>();
    }
}
