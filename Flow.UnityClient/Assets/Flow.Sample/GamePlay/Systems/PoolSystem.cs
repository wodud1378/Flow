using System;
using System.Collections.Generic;
using Flow.Sample.GamePlay.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Flow.Sample.GamePlay.Systems
{
    public class PoolSystem
    {
        private readonly Dictionary<object, object> _pools = new();
        private readonly Dictionary<GameObject, object> _keyLookup = new();

        public T GetObject<T>(T prefab, Func<T, T> customInstantiate = null) where T : Component
        {
            var pool = GetPool(prefab, customInstantiate);
            return pool?.Get();
        }
        
        public void Destroy<T>(T entity) where T : Component
        {
            if (!_keyLookup.TryGetValue(entity.gameObject, out var poolObj) ||
                poolObj is not ObjectPool<T> pool)
                return;
            
            pool.Release(entity);
        }

        private ObjectPool<T> GetPool<T>(T prefab, Func<T, T> customInstantiate = null) where T : Component
        {
            if (_pools.TryGetValue(prefab, out var obj))
                return obj as ObjectPool<T>;

            var pool = new ObjectPool<T>(prefab, customInstantiate ?? CreateInstance);
            _pools[prefab] = pool;
            _keyLookup[prefab.gameObject] = pool;
            return pool;
        }


        private T CreateInstance<T>(T prefab) where T : Component => Object.Instantiate(prefab);
    }
}