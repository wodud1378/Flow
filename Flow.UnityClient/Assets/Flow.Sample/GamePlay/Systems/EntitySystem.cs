using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;

namespace Flow.Sample.GamePlay.Systems
{
    public class EntitySystem : IEntityContainer
    {
        public IReadOnlyDictionary<int, BaseEntity> Entities => _entities;

        private readonly Dictionary<int, BaseEntity> _entities = new();
        private readonly Dictionary<Type, HashSet<BaseEntity>> _typeIndex = new();

        private readonly Dictionary<Type, Type[]> _interfaceCache = new();
        private readonly List<HashSet<BaseEntity>> _hashSetsBuffer = new();
        private readonly HashSet<BaseEntity> _hashSetBuffer = new();
        

        public BaseEntity GetEntity(int id) => _entities.GetValueOrDefault(id);

        public IEnumerable<T> GetEntities<T>() where T : BaseEntity
        {
            return _typeIndex.TryGetValue(typeof(T), out var entities)
                ? entities.Cast<T>()
                : Array.Empty<T>();
        }

        public int GetEntities(Span<BaseEntity> buffer, params Type[] types)
        {
            _hashSetBuffer.Clear();
            _hashSetsBuffer.Clear();

            if (types == null || types.Length == 0 || buffer.Length == 0)
                return 0;

            var baseType = typeof(IComponent);
            foreach (var type in types)
            {
                if (!baseType.IsAssignableFrom(type))
                    continue;

                if (!_typeIndex.TryGetValue(type, out var entities))
                    return 0;

                _hashSetsBuffer.Add(entities);
            }

            if (_hashSetsBuffer.Count == 0)
                return 0;

            _hashSetsBuffer.Sort((a, b) => a.Count.CompareTo(b.Count));
            _hashSetBuffer.UnionWith(_hashSetsBuffer[0]);

            int count = _hashSetsBuffer.Count;
            int entityCount = _hashSetBuffer.Count;
            for (int i = 1; i < count && entityCount > 0; ++i)
            {
                _hashSetBuffer.IntersectWith(_hashSetsBuffer[i]);
                entityCount = _hashSetBuffer.Count;
            }

            if (entityCount == 0)
                return 0;

            int capacity = Math.Min(entityCount, buffer.Length);
            for (int i = 0; i < capacity; i++)
            {
                buffer[i] = _hashSetBuffer.ElementAt(i);
            }

            return capacity;
        }

        public void Register<T>(T entity) where T : BaseEntity
        {
            _entities[entity.Id] = entity;
            var components = entity.GetSystemComponents();
            foreach (var component in components)
            {
                RegisterToTypeIndex(component.GetType(), entity);
            }
        }

        public void Unregister<T>(T entity) where T : BaseEntity
        {
            _entities.Remove(entity.Id);

            var components = entity.GetSystemComponents();
            foreach (var component in components)
            {
                UnregisterFromTypeIndex(component.GetType(), entity);
            }
        }

        private void RegisterToTypeIndex(Type type, BaseEntity baseEntity)
        {
            if (!_typeIndex.TryGetValue(type, out var list))
            {
                list = new HashSet<BaseEntity>();
                _typeIndex[type] = list;
            }

            list.Add(baseEntity);
        }

        private void UnregisterFromTypeIndex(Type type, BaseEntity baseEntity)
        {
            if (!_typeIndex.TryGetValue(type, out var set))
                return;

            set.Remove(baseEntity);

            if (set.Count == 0)
            {
                _typeIndex.Remove(type);
            }
        }

        private Type[] GetInterfacesCached(Type type)
        {
            if (!_interfaceCache.TryGetValue(type, out var interfaces))
            {
                interfaces = type.GetInterfaces();
                _interfaceCache[type] = interfaces;
            }

            return interfaces;
        }
    }
}