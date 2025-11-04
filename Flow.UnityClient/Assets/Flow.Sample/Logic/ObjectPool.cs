using System;
using System.Collections.Generic;
using Flow.Sample.Logic.Interfaces;
using UnityEngine;

namespace Flow.Sample.Logic
{
    public class ObjectPool<T> where T : IPoolItem
    {
        private readonly Func<T> _createInstance;
        private readonly List<T> _activated = new();
        private readonly List<T> _spares = new();

        private readonly GameObject _prefab;

        private bool HasSpare => _spares.Count > 0;

        public ObjectPool(Func<T> createInstance)
        {
            _createInstance = createInstance;
        }

        public T Get(Action<T> onBeforeActive = null)
        {
            T obj;
            if (HasSpare)
            {
                obj = _spares[0];
                _spares.RemoveAt(0);
            }
            else
            {
                obj = _createInstance.Invoke();
            }

            onBeforeActive?.Invoke(obj);
            
            obj.Activate();
            _activated.Add(obj);

            return obj;
        }

        public void Release(T obj)
        {
            obj.Deactivate();

            _activated.Remove(obj);
            _spares.Add(obj);
        }
    }
}