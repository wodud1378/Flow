using System;
using Flow.Sample.Entities;
using Flow.Sample.Logic.Interfaces;
using UnityEngine;

namespace Flow.Sample.View.Contents.Attack
{
    public class Projectile : MonoBehaviour, IPoolItem
    {
        public event Action OnReachedTarget;

        [SerializeField] private float speed;
        [SerializeField] private float reachThreshold = 0.1f;

        private BaseEntity _target;
        private Vector2 _targetPosition;
        private float _reachThreshold;
        private bool _launched;

        public void Launch(Vector2 startPosition, BaseEntity target)
        {
            transform.position = startPosition;

            _target = target;
            _launched = _target != null && _target.IsValid;
            _reachThreshold = Mathf.Pow(reachThreshold, 2);
        }

        public void ManualUpdate(float deltaTime)
        {
            if (!_launched)
                return;

            if (!MoveToTarget(deltaTime))
                return;

            OnReachedTarget?.Invoke();
            _launched = false;
        }

        private bool MoveToTarget(float deltaTime)
        {
            UpdateTargetPosition();

            var current = (Vector2)transform.position;
            var step = Mathf.Pow(speed * deltaTime, 2);
            var pos = Vector2.MoveTowards(current, _targetPosition, step);
            transform.position = pos;

            var diff = _targetPosition - pos;
            var distance = diff.magnitude;
            return distance <= _reachThreshold;
        }

        private void UpdateTargetPosition()
        {
            _targetPosition = (_target != null && _target.IsValid)
                ? _target.transform.position
                : _targetPosition;
        }

        public void Activate() => gameObject.SetActive(true);

        public void Deactivate()
        {
            OnReachedTarget = null;
            
            gameObject.SetActive(false);
        }
    }
}