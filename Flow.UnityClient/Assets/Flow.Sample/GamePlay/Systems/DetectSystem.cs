using System;
using System.Collections.Generic;
using Flow.Sample.GamePlay.Systems.Models;
using UnityEngine;

namespace Flow.Sample.GamePlay.Systems
{
    public class DetectSystem
    {
        private readonly ComponentCacheSystem _cache;
        private readonly Vector2[] _arcCheckBuffer = new Vector2[4];

        private readonly Stack<Collider2D[]> _overlapBufferPool = new();
        private readonly Stack<Component[]> _componentBufferPool = new();

        public int BufferSize { get; set; }

        public DetectSystem(ComponentCacheSystem cache, int bufferSize = 32)
        {
            _cache = cache;
            BufferSize = bufferSize;
        }

        public DetectScope Detect(IDetectParams param)
        {
            return param switch
            {
                ArcParams arcParams => DetectArc(arcParams),
                BoxParams boxParams => DetectBox(boxParams),
                CircleParams circleParams => DetectCircle(circleParams),
                _ => new (
                    this,
                    null,
                    ReadOnlySpan<Collider2D>.Empty
                )
            };
        }

        public DetectScope<T> Detect<T>(IDetectParams param) where T : Component
        {
            using var scope = Detect(param);
            
            var buffer = RentComponentBuffer();
            int bufferIndex = 0;
            foreach (var collider in scope.Detected)
            {
                if (!_cache.TryGetComponent<T>(collider.gameObject, out var component))
                    continue;

                buffer[bufferIndex++] = component;
            }

            return new(
                this,
                buffer,
                buffer.AsSpan(0, bufferIndex)
            );
        }

        private DetectScope DetectBox(BoxParams param)
        {
            var buffer = RentOverlapBuffer();
            var found = Physics2D.OverlapBox(param.Point, param.Size, param.Angle, param.Filter, buffer);
            return new (
                this,
                buffer,
                buffer.AsSpan(0, found)
            );
        }

        private DetectScope DetectArc(ArcParams param)
        {
            int found = DetectCircle(param.Point, param.Radius, param.Filter, out var buffer);
            int bufferIndex = 0;
            for (int i = 0; i < found; ++i)
            {
                var collider = buffer[i];
                if (!InBound(param, collider))
                    continue;

                buffer[bufferIndex++] = collider;
            }

            return new(
                this,
                buffer,
                buffer.AsSpan(0, bufferIndex)
            );
        }

        private DetectScope DetectCircle(CircleParams param)
        {
            var found = DetectCircle(param.Point, param.Radius, param.Filter, out var buffer);
            return new(
                this,
                buffer,
                buffer.AsSpan(0, found)
            );
        }

        private int DetectCircle(Vector2 point, float radius, ContactFilter2D filter, out Collider2D[] buffer)
        {
            buffer = RentOverlapBuffer();
            return Physics2D.OverlapCircle(point, radius, filter, buffer);
        }

        private bool InBound(ArcParams param, Collider2D collider)
        {
            if (collider == null)
                return false;

            var targetPos = (Vector2)collider.transform.position - param.Point;
            var halfSize = collider.bounds.size * 0.5f;

            // Left Up
            _arcCheckBuffer[0].Set(targetPos.x - halfSize.x, targetPos.y + halfSize.y);
            // Right Up
            _arcCheckBuffer[1].Set(targetPos.x + halfSize.x, targetPos.y + halfSize.y);
            // Left Bottom
            _arcCheckBuffer[2].Set(targetPos.x - halfSize.x, targetPos.y - halfSize.y);
            // Right Bottom
            _arcCheckBuffer[3].Set(targetPos.x + halfSize.x, targetPos.y - halfSize.y);

            var halfAngle = param.Angle * 0.5f;
            float cosHalf = Mathf.Cos(halfAngle * Mathf.Deg2Rad);
            foreach (var buffer in _arcCheckBuffer)
            {
                var dot = Vector2.Dot(buffer.normalized, param.Forward);
                if (dot >= cosHalf)
                    return true;
            }

            return false;
        }

        private Collider2D[] RentOverlapBuffer()
        {
            var buffer = _overlapBufferPool.Count > 0
                ? _overlapBufferPool.Pop()
                : new Collider2D[BufferSize];

            if (buffer.Length < BufferSize)
                Array.Resize(ref buffer, BufferSize);

            return buffer;
        }

        private Component[] RentComponentBuffer()
        {
            var buffer = _componentBufferPool.Count > 0
                ? _componentBufferPool.Pop()
                : new Component[BufferSize];

            if (buffer.Length < BufferSize)
                Array.Resize(ref buffer, BufferSize);

            return buffer;
        }

        internal void ReturnOverlapBuffer(Collider2D[] buffer) => _overlapBufferPool.Push(buffer);

        internal void ReturnComponentBuffer(Component[] buffer) => _componentBufferPool.Push(buffer);
    }
}