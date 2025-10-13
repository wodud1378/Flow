using System;
using System.Collections.Generic;

namespace Flow.Sample.Entities.Interfaces
{
    public interface IEntityContainer
    {
        public IReadOnlyDictionary<int, BaseEntity> Entities { get; }
        public BaseEntity GetEntity(int id);
        public IEnumerable<T> GetEntities<T>() where T : BaseEntity;
        public int GetEntities(Span<BaseEntity> buffer, params Type[] types);
        
        public void Register<T>(T entity) where T : BaseEntity;
        public void Unregister<T>(T entity) where T : BaseEntity;
    }
}