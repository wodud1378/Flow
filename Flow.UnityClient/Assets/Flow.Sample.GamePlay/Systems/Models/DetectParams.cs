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

        public ArcParams Copy(
            float? angle = null,
            float? radius = null,
            Vector2? forward = null,
            Vector2? point = null,
            ContactFilter2D? filter = null)
        {
            return new ArcParams(
                angle ?? Angle,
                radius ?? Radius,
                forward ?? Forward,
                point ?? Point,
                filter ?? Filter
            );
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

        public CircleParams Copy(
            float? radius = null,
            Vector2? point = null,
            ContactFilter2D? filter = null)
        {
            return new CircleParams(
                radius ?? Radius,
                point ?? Point,
                filter ?? Filter
            );
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

        public BoxParams Copy(
            Vector2? size = null,
            float? angle = null,
            Vector2? point = null,
            ContactFilter2D? filter = null)
        {
            return new BoxParams(
                size ?? Size,
                angle ?? Angle,
                point ?? Point,
                filter ?? Filter
            );
        }
    }
}