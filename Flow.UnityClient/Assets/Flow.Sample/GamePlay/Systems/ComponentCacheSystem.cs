using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Flow.Sample.Entities;
using Flow.Sample.GamePlay.Components.Interfaces;
using Flow.Sample.GamePlay.Systems.Interfaces;
using UnityEngine;

namespace Flow.Sample.GamePlay.Systems
{
    public class ComponentCacheSystem : IComponentProvider
    {
        private readonly Dictionary<GameObject, Dictionary<Type, Component>> _componentCache = new();
        private readonly Dictionary<BaseEntity, Dictionary<Type, IComponent>> _entityCache = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetComponent<T>(GameObject obj) where T : Component
        {
            if (!_componentCache.TryGetValue(obj, out var components))
            {
                components = new Dictionary<Type, Component>();
                _componentCache[obj] = components;
            }

            var type = typeof(T);
            if (!components.TryGetValue(type, out var component))
            {
                if (obj.TryGetComponent(type, out component))
                {
                    components[type] = component;
                }
            }

            return (T)component;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetComponent<T>(GameObject obj, out T component) where T : Component
        {
            component = GetComponent<T>(obj);
            return component != null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetComponent<T>(BaseEntity entity) where T : class, IComponent
        {
            if (!_entityCache.TryGetValue(entity, out var components))
            {
                components = new Dictionary<Type, IComponent>();
                _entityCache[entity] = components;
            }

            var type = typeof(T);
            if (!components.TryGetValue(type, out var component))
            {
                if (entity.gameObject.TryGetComponent(out component))
                {
                    components[type] = component;
                }
            }

            return (T)component;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetComponent<T>(BaseEntity entity, out T component) where T : class, IComponent
        {
            component = GetComponent<T>(entity);
            return component != null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IComponent[] GetComponents<T>(T entity) where T : BaseEntity
        {
            var components = entity.GetComponents<IComponent>();
            foreach (var component in components)
            {
                _entityCache[entity][component.GetType()] = component;
            }

            return components;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TComponent[] GetComponents<T, TComponent>(T entity) where T : BaseEntity where TComponent : class, IComponent
        {
            var components = entity.GetComponents<TComponent>();
            foreach (var component in components)
            {
                _entityCache[entity][component.GetType()] = component;
            }

            return components;
        }

        public void Clear(BaseEntity entity) => _componentCache.Remove(entity.gameObject);
    }
}