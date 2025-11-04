using System.Threading;
using Flow.Sample.GamePlay.Components.Interfaces;
using Flow.Sample.GamePlay.Systems.Interfaces;
using Flow.Sample.Logic.Interfaces;
using UnityEngine;

namespace Flow.Sample.Entities
{
    public abstract class BaseEntity : MonoBehaviour, IPoolItem
    {
        public int Id { get; private set; }
        public bool IsValid { get; private set; }

        public bool DestroyTriggered { get; private set; }
        
        public GameObject Origin { get; set; }
        public bool IsActive => gameObject.activeSelf;

        private CancellationToken _lifeTimeCt;
        private IComponentProvider _componentProvider;

        public virtual void Initialize(int id, IComponentProvider componentProvider = null)
        {
            Id = id;
            IsValid = true;

            _componentProvider = componentProvider;
        }

        public virtual void Invalidate()
        {
            IsValid = false;
        }

        public new T GetComponent<T>() where T : Component => 
            _componentProvider?.GetComponent<T>(gameObject) ?? base.GetComponent<T>();

        public new bool TryGetComponent<T>(out T component) where T : Component => 
            _componentProvider?.TryGetComponent(gameObject, out component) ?? base.TryGetComponent(out component);

        public T GetSystemComponent<T>() where T : class, IComponent =>
            _componentProvider?.GetComponent<T>(this) ?? base.GetComponent<T>();

        public bool TryGetSystemComponent<T>(out T component) where T : class, IComponent =>
            _componentProvider?.TryGetComponent(this, out component) ?? base.TryGetComponent(out component);

        public IComponent[] GetComponents() => _componentProvider?.GetComponents(this) ?? GetComponents<IComponent>();
        
        public new TComponent[] GetComponents<TComponent>() where TComponent : class, IComponent =>
            _componentProvider?.GetComponents<BaseEntity, TComponent>(this) ?? base.GetComponents<TComponent>();
            

        public void DestroySelf() => DestroyTriggered = true;

        public void CancelDestroySelf() => DestroyTriggered = false;

        public virtual void Activate() => gameObject.SetActive(true);

        public virtual void Deactivate() => gameObject.SetActive(false);
    }
}