using System;
using System.Collections.Generic;
using Flow.Sample.Entities;
using Flow.Sample.Logic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Flow.Sample.GamePlay.Systems
{
    public class EntityPoolSystem
    {
        private readonly Dictionary<object, object> _pools = new();
        private readonly Dictionary<object, object> _accessors = new();
        
        public T GetObject<T>(T prefab, Action<T> onBeforeActive = null) where T : BaseEntity
        {
            var pool = GetPool(prefab);
            if (pool == null)
                return null;

            var t = pool.Get(onBeforeActive);
            if (t == null)
                return null;
            
            if(t.DestroyTriggered)
                t.CancelDestroySelf();

            return t;
        }

        private ObjectPool<T> GetPool<T>(T prefab) where T : BaseEntity
        {
            if(_pools.TryGetValue(prefab, out var obj))
                return obj as ObjectPool<T>;
            
            var pool = new ObjectPool<T>(() => CreateInstance(prefab));
            _pools[prefab] = pool;
            return pool;
        }

        public void Destroy<T>(T entity) where T : BaseEntity
        {
            var pool = GetPool(entity);
            pool?.Release(entity);
        }

        private T CreateInstance<T>(T prefab) where T : MonoBehaviour => Object.Instantiate(prefab);
    }
}