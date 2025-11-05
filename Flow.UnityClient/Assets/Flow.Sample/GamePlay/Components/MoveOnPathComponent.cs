using Flow.Sample.Entities;
using Flow.Sample.GamePlay.Components.Interfaces;
using Flow.Sample.GamePlay.Models;
using UnityEngine;

namespace Flow.Sample.GamePlay.Components
{
    public class MoveOnPathComponent : MonoBehaviour, IComponent
    {
        [field: SerializeField] public BaseEntity Owner { get; private set; }
        
        [field: SerializeField] public float Speed { get; set; }

        [SerializeField] private float nextPointThreshold = 0.1f;

        private MovePath _path;
        private int _pathIndex;
        private bool _initialized;
        
        public void OnValidate()
        {
            if(Owner == null)
                Owner = GetComponent<BaseEntity>();
        }
        
        public void Initialize(MovePath path)
        {
            _path = path;
            _initialized = _path.Points != null;

            if (!_initialized)
                return;

            _pathIndex = 0;
            transform.position = _path.Points![_pathIndex];
            nextPointThreshold = Mathf.Pow(nextPointThreshold, 2);
        }

        public void UpdateMove(float deltaTime)
        {
            if (_path.Points.Length < 2 || _pathIndex >= _path.Points.Length)
                return;

            var current = (Vector2)transform.position;
            var target = _path.Points[_pathIndex];
            var step = Mathf.Pow(Speed * deltaTime, 2);
            var diff = target - current;
            var distance = diff.sqrMagnitude;
            if (distance < nextPointThreshold)
            {
                ++_pathIndex;
                return;
            }
            
            transform.position = Vector2.MoveTowards(current, target, step);
        }
    }
}