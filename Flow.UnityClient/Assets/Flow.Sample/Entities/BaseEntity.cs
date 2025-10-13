using System;
using System.Collections.Generic;
using Flow.Sample.GamePlay.Components;
using UnityEngine;

namespace Flow.Sample.Entities
{
    public abstract class BaseEntity : MonoBehaviour
    {
        private readonly Dictionary<Type, IComponent> _componentCache = new();
        
        public int Id { get; private set; }
        public bool IsValid { get; private set; }

        public virtual void Initialize(int id)
        {
            Id = id;
            IsValid = true;
        }

        public virtual void Invalidate()
        {
            IsValid = false;
        }

        public new T GetComponent<T>() where T : IComponent
        {
            var type = typeof(T);
            if (!_componentCache.TryGetValue(type, out var component))
            {
                component = base.GetComponent<T>();
                if (component != null)
                {
                    _componentCache[type] = component;
                }
            }

            return (T)component;
        }

        public new bool TryGetComponent<T>(out T component) where T : IComponent
        {
            component = GetComponent<T>();
            return component != null;
        }
        
        public IComponent[] GetSystemComponents()
        {
            var components = GetComponents<IComponent>();
            foreach (var component in components)
            {
                _componentCache[component.GetType()] = component;
            }

            return components;
        }
    }
}