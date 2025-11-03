using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Flow.Sample.Logic
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        public event Action<T> OnBeforeActive;
        
        private readonly List<T> _activated = new();
        private readonly List<T> _spares = new();
        
        private readonly GameObject _prefab;

        private bool HasSpare => _spares.Count > 0;
        
        public ObjectPool(GameObject prefab)
        {
            _prefab = prefab;
        }

        public T Get(Vector2 position = default)
        {
            T obj;
            if (HasSpare)
            {
                obj = _spares[0];
                _spares.RemoveAt(0);
            }
            else
            {
                var go = Object.Instantiate(_prefab);
                obj = go.GetComponent<T>();
            }

            OnBeforeActive?.Invoke(obj);
            
            obj.transform.position = position;
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