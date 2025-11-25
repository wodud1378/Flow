using System.Collections.Generic;
using Flow.Sample.GamePlay.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Flow.Sample.GamePlay.Systems
{
    public class PoolSystem
    {
        private readonly Dictionary<object, object> _pools = new();
        
        public T GetObject<T>(T prefab) where T : MonoBehaviour
        {
            var pool = GetPool(prefab);
            return pool?.Get();
        }

        private ObjectPool<T> GetPool<T>(T prefab) where T : MonoBehaviour
        {
            if(_pools.TryGetValue(prefab, out var obj))
                return obj as ObjectPool<T>;
            
            var pool = new ObjectPool<T>(() => CreateInstance(prefab));
            _pools[prefab] = pool;
            return pool;
        }

        public void Destroy<T>(T entity) where T : MonoBehaviour
        {
            var pool = GetPool(entity);
            pool?.Release(entity);
        }

        private T CreateInstance<T>(T prefab) where T : MonoBehaviour => Object.Instantiate(prefab);
    }
}