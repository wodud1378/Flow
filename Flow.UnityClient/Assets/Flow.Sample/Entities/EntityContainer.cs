using System;
using System.Collections.Generic;
using System.Linq;
using Flow.Sample.Entities.Interfaces;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Components.Interfaces;
using Flow.Sample.GamePlay.Systems.Interfaces;
using UnityEngine;

namespace Flow.Sample.Entities
{
    public class EntityContainer : IEntityContainer
    {
        public IReadOnlyDictionary<int, BaseEntity> Entities => _entities;

        private readonly Dictionary<int, BaseEntity> _entities = new();
        private readonly Dictionary<Type, HashSet<BaseEntity>> _typeIndex = new();

        private readonly List<HashSet<BaseEntity>> _hashSetsBuffer = new();
        private readonly HashSet<BaseEntity> _hashSetBuffer = new();

        private readonly IComponentProvider _componentProvider;

        public EntityContainer(IComponentProvider componentProvider)
        {
            _componentProvider = componentProvider;
        }

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

            if (buffer.Length == 0)
                return 0;

            if (types == null || types.Length == 0)
                return GetAllEntities(buffer);

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

        private int GetAllEntities(Span<BaseEntity> buffer)
        {
            int count = Mathf.Min(buffer.Length, _entities.Count);
            int currentCount = 0;
            foreach (var kvp in _entities)
            {
                buffer[currentCount] = kvp.Value;
                ++currentCount;

                if (currentCount >= count)
                    break;
            }

            return count;
        }

        public void Register<T>(T entity) where T : BaseEntity
        {
            _entities[entity.Id] = entity;
            
            var components = _componentProvider.GetComponents(entity);
            foreach (var component in components)
            {
                RegisterToTypeIndex(component.GetType(), entity);
            }
        }

        public void Unregister<T>(T entity) where T : BaseEntity
        {
            _entities.Remove(entity.Id);
            
            var components = _componentProvider.GetComponents(entity);
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
    }
}