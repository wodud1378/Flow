using System;
using System.Collections.Generic;
using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Components.Interfaces;
using Flow.Sample.GamePlay.Systems.Base;
using UnityEngine;

namespace Flow.Sample.GamePlay.Systems
{
    public class ComponentCacheSystem : BaseUpdateEntitySystem, IComponentProvider
    {
        private readonly Dictionary<GameObject, Dictionary<Type, Component>> _componentCache = new();
        private readonly Dictionary<BaseEntity, Dictionary<Type, IComponent>> _entityCache = new();

        protected override Type[] EntityFilter => null;

        public ComponentCacheSystem(IEntityContainer entityContainer) : base(entityContainer, 256)
        {
        }

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

        public bool TryGetComponent<T>(GameObject obj, out T component) where T : Component
        {
            component = GetComponent<T>(obj);
            return component != null;
        }

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
                if (entity.TryGetComponent(out component))
                {
                    components[type] = component;
                }
            }

            return (T)component;
        }

        public bool TryGetComponent<T>(BaseEntity entity, out T component) where T : class, IComponent
        {
            component = GetComponent<T>(entity);
            return component != null;
        }

        public IComponent[] GetComponents<T>(T entity) where T : BaseEntity
        {
            var components = entity.GetComponents<IComponent>();
            foreach (var component in components)
            {
                _entityCache[entity][component.GetType()] = component;
            }

            return components;
        }

        protected override void OnUpdateEntity(BaseEntity baseEntity, int index, float deltaTime)
        {
            if (baseEntity.DestroyTriggered)
                return;

            _componentCache.Remove(baseEntity.gameObject);
        }
    }
}