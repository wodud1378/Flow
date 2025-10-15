using System;
using System.Collections.Generic;
using System.Linq;
using Flow.Sample.Entities.Interfaces;

namespace Flow.Sample.Entities
{
    public class EntityContainer : IEntityContainer
    {
        private readonly Dictionary<int, BaseEntity> _entities = new();
        private readonly Dictionary<Type, List<BaseEntity>> _entitiesByType = new();
        
        public IReadOnlyDictionary<int, BaseEntity> Entities => _entities;
        
        public BaseEntity GetEntity(int id)
        {
            return _entities.TryGetValue(id, out var entity) ? entity : null;
        }
        
        public IEnumerable<T> GetEntities<T>() where T : BaseEntity
        {
            var type = typeof(T);
            if (_entitiesByType.TryGetValue(type, out var list))
            {
                return list.Cast<T>().Where(e => e.IsValid);
            }
            return Enumerable.Empty<T>();
        }
        
        public int GetEntities(Span<BaseEntity> buffer, params Type[] types)
        {
            int count = 0;
            
            foreach (var type in types)
            {
                if (_entitiesByType.TryGetValue(type, out var list))
                {
                    foreach (var entity in list)
                    {
                        if (entity.IsValid && count < buffer.Length)
                        {
                            buffer[count++] = entity;
                        }
                    }
                }
            }
            
            return count;
        }
        
        public void Register<T>(T entity) where T : BaseEntity
        {
            if (entity == null) return;
            
            _entities[entity.Id] = entity;
            
            var type = entity.GetType();
            if (!_entitiesByType.ContainsKey(type))
            {
                _entitiesByType[type] = new List<BaseEntity>();
            }
            _entitiesByType[type].Add(entity);
        }
        
        public void Unregister<T>(T entity) where T : BaseEntity
        {
            if (entity == null) return;
            
            _entities.Remove(entity.Id);
            
            var type = entity.GetType();
            if (_entitiesByType.TryGetValue(type, out var list))
            {
                list.Remove(entity);
            }
        }
    }
}
