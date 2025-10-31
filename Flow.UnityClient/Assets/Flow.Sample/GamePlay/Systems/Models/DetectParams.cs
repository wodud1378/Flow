using UnityEngine;

namespace Flow.Sample.GamePlay.Systems.Models
{
    public interface IDetectParams
    {
        public Vector2 Point { get; }
        public ContactFilter2D Filter { get; }
    }
    
    public readonly struct ArcParams : IDetectParams
    {
        public float Angle { get; }
        public float Radius { get; }
        public Vector2 Forward { get; }
        public Vector2 Point { get; }
        public ContactFilter2D Filter { get; }
        
        public ArcParams(float angle, float radius, Vector2 forward, Vector2 point, ContactFilter2D filter)
        {
            Angle = angle;
            Radius = radius;
            Forward = forward;
            Point = point;
            Filter = filter;
        }
    }
    
    public readonly struct CircleParams : IDetectParams
    {
        public float Radius { get; }
        public Vector2 Point { get; }
        public ContactFilter2D Filter { get; }
        
        public CircleParams(float radius, Vector2 point, ContactFilter2D filter)
        {
            Radius = radius;
            Point = point;
            Filter = filter;
        }
    }
    
    public readonly struct BoxParams : IDetectParams
    {
        public float Angle { get; }
        public Vector2 Size { get; }
        public Vector2 Point { get; }
        public ContactFilter2D Filter { get; }
        
        public BoxParams(Vector2 size, float angle, Vector2 point, ContactFilter2D filter)
        {
            Size = size;
            Angle = angle;
            Point = point;
            Filter = filter;
        }
    }
}