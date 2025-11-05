using UnityEngine;

namespace Flow.Sample.GamePlay.Models
{
    public readonly struct MovePath
    {
        public readonly Vector2[] Points;

        public MovePath(Vector2[] points)
        {
            Points = points;
        }
    }
}