using System;
using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;

namespace Flow.Sample.GamePlay.Systems.Base
{
    public abstract class BaseUpdateEntitySystem : BaseSystem
    {
        public const int DefaultBufferSize = 32;
        
        protected abstract Type[] EntityFilter { get; }
        
        private readonly IEntityContainer _entityContainer;
        private readonly BaseEntity[] _entityBuffer;

        protected BaseUpdateEntitySystem(IEntityContainer entityContainer, int bufferSize = DefaultBufferSize)
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
        
        protected abstract void OnUpdateEntity(BaseEntity baseEntity, int index, float deltaTime);
    }
}