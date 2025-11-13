using Flow.Sample.Entities;
using Flow.Sample.GamePlay.Components.Interfaces;
using UnityEngine;

namespace Flow.Sample.GamePlay.Systems.Interfaces
{
    public interface IComponentProvider
    {
        public T GetComponent<T>(GameObject gameObject);
        public bool TryGetComponent<T>(GameObject obj, out T component);

        public T GetComponent<T>(BaseEntity entity) where T : class, IComponent;
        public bool TryGetComponent<T>(BaseEntity entity, out T component) where T : class, IComponent;
        public IComponent[] GetComponents<T>(T entity) where T : BaseEntity;

        public TComponent[] GetComponents<T, TComponent>(T entity)
            where T : BaseEntity where TComponent : class, IComponent;
    }
}