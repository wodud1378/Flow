using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Flow.Sample.GamePlay.Components.Interfaces;
using UnityEngine;

namespace Flow.Sample.GamePlay.Entities
{
    public abstract class BaseEntity : MonoBehaviour
    {
        public int Id { get; private set; }
        public bool IsValid { get; private set; }
        public bool DestroyTriggered { get; private set; }

        private Dictionary<Type, object> _cache;

        public virtual void Initialize(int id)
        {
            Id = id;
            IsValid = true;
            _cache ??= new Dictionary<Type, object>();
        }

        public virtual void Invalidate()
        {
            IsValid = false;
            _cache?.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new T GetComponent<T>() where T : Component
        {
            _cache ??= new Dictionary<Type, object>();

            var type = typeof(T);
            if (_cache.TryGetValue(type, out var cached))
                return (T)cached;

            var component = base.GetComponent<T>();
            if (component != null)
                _cache[type] = component;

            return component;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new bool TryGetComponent<T>(out T component) where T : Component
        {
            component = GetComponent<T>();
            return component != null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetSystemComponent<T>() where T : class, IComponent
        {
            _cache ??= new Dictionary<Type, object>();

            var type = typeof(T);
            if (_cache.TryGetValue(type, out var cached))
                return (T)cached;

            var component = base.GetComponent<T>();
            if (component != null)
                _cache[type] = component;

            return component;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetSystemComponent<T>(out T component) where T : class, IComponent
        {
            component = GetSystemComponent<T>();
            return component != null;
        }

        public IComponent[] GetSystemComponents()
        {
            var components = base.GetComponents<IComponent>();
            CacheComponents(components);
            return components;
        }

        public new TComponent[] GetComponents<TComponent>() where TComponent : class, IComponent
        {
            var components = base.GetComponents<TComponent>();
            CacheComponents(components);
            return components;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CacheComponents<T>(T[] components)
        {
            _cache ??= new Dictionary<Type, object>();
            foreach (var component in components)
                _cache[component.GetType()] = component;
        }

        public void DestroySelf() => DestroyTriggered = true;

        public void CancelDestroySelf() => DestroyTriggered = false;

        public virtual void Activate() => gameObject.SetActive(true);

        public virtual void Deactivate() => gameObject.SetActive(false);
    }
}
