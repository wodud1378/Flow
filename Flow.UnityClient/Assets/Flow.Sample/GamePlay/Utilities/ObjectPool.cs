using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flow.Sample.GamePlay.Utilities
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private readonly List<T> _activated = new();
        private readonly List<T> _spares = new();

        private readonly T _prefab;
        private readonly Func<T, T> _createInstance;

        private bool HasSpare => _spares.Count > 0;

        public ObjectPool(T prefab, Func<T, T> createInstance)
        {
            _prefab = prefab;
            _createInstance = createInstance;
        }

        public T Get()
        {
            T obj;
            if (HasSpare)
            {
                obj = _spares[0];
                _spares.RemoveAt(0);
            }
            else
            {
                obj = _createInstance.Invoke(_prefab);
            }
            
            obj.gameObject.SetActive(true);
            _activated.Add(obj);

            return obj;
        }

        public void Release(T obj)
        {
            obj.gameObject.SetActive(false);

            _activated.Remove(obj);
            _spares.Add(obj);
        }
    }
}