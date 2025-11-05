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
        
        public bool ReachedToEnd { get; private set; }

        private MovePath _path;
        private int _pathIndex;
        private bool _initialized;

        public void OnValidate()
        {
            if (Owner == null)
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
            
            ReachedToEnd = false;
        }

        public void UpdateMove(float deltaTime)
        {
            if (ReachedToEnd)
                return;
            
            if (_path.Points.Length < 2)
                return;

            var current = (Vector2)transform.position;
            var target = _path.Points[_pathIndex];
            var step = Mathf.Pow(Speed * deltaTime, 2);
            var diff = target - current;
            var distance = diff.sqrMagnitude;
            if (distance <= nextPointThreshold)
            {
                var nextIndex = _pathIndex + 1;
                if (nextIndex < _path.Points.Length)
                    _pathIndex = nextIndex;
                else
                    ReachedToEnd = true;

                return;
            }

            var pos = Vector2.MoveTowards(current, target, step);
            transform.position = pos;

            UpdateRotation(pos, deltaTime);
        }

        private void UpdateRotation(Vector2 position, float deltaTime)
        {
            var angle = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.Euler(0f, 0f, angle);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                rotation,
                // 1초에 360도 회전 가능.
                360f * deltaTime
            );
        }
    }
}