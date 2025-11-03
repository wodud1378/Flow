using System;
using System.Collections.Generic;
using Flow.Sample.Entities;
using Flow.Sample.GamePlay.Systems.Interfaces;
using Flow.Sample.Logic;
using UnityEngine;

namespace Flow.Sample.GamePlay.Systems
{
    public class PoolSystem
    {
        private readonly Dictionary<Type, ObjectPool<BaseEntity>> _pools = new();

        public void Register<T>(GameObject prefab) where T : BaseEntity
        {
            var type = typeof(T);
            if (!_pools.ContainsKey(type))
                return;
            
            _pools[type] = new ObjectPool<BaseEntity>(prefab);
        }
        
        public T GetObject<T>(Vector2 position = default) where T : BaseEntity
        {
            if (!_pools.TryGetValue(typeof(T), out var pool))
                return null;

            var t = pool.Get(position) as T;
            if (t == null)
                return null;
            
            if(t.DestroyTriggered)
                t.CancelDestroySelf();

            return t;
        }

        public void Destroy<T>(T entity) where T : BaseEntity
        {
            if (!_pools.TryGetValue(typeof(T), out var pool))
                return;
            
            pool.Release(entity);
        }
    }
}