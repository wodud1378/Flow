using System;
using UnityEngine;

namespace Flow.Sample.GamePlay.Systems.Models
{
    public readonly ref struct DetectScope
    {
        private readonly DetectSystem _owner;
        private readonly Collider2D[] _buffer;
        
        public readonly ReadOnlySpan<Collider2D> Detected;
        
        internal DetectScope(DetectSystem owner, Collider2D[] buffer, ReadOnlySpan<Collider2D> detected)
        {
            _owner = owner;
            _buffer = buffer;
            Detected = detected;
        }
        
        public void Dispose()
        {
            if (_buffer == null)
                return;
            
            _owner.ReturnOverlapBuffer(_buffer);
        }
    }
    
    public readonly ref struct DetectScope<T>
    {
        private readonly DetectSystem _owner;
        private readonly Component[] _buffer;
        
        public readonly ReadOnlySpan<Component> Detected;
        
        internal DetectScope(DetectSystem owner, Component[] buffer, ReadOnlySpan<Component> detected)
        {
            _owner = owner;
            _buffer = buffer;
            Detected = detected;
        }
        
        public void Dispose()
        {
            if (_buffer == null)
                return;
            
            _owner.ReturnComponentBuffer(_buffer);
        }
    }
}