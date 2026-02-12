using System.Collections.Generic;
using Flow.Sample.GamePlay.Models;
using Flow.Sample.GamePlay.Systems.Interfaces;
using UnityEngine;

namespace Flow.Sample.GamePlay.Providers
{
    public class MovePathProvider : MonoBehaviour, IMovePathProvider
    {
        [SerializeField] private List<Transform> waypoints = new();

        private MovePath _cachedPath;
        private bool _pathCached;

        public MovePath Provide()
        {
            if (_pathCached)
                return _cachedPath;

            var points = new Vector2[waypoints.Count];
            for (int i = 0; i < waypoints.Count; i++)
            {
                points[i] = waypoints[i].position;
            }

            _cachedPath = new MovePath(points);
            _pathCached = true;

            return _cachedPath;
        }

        public void InvalidateCache()
        {
            _pathCached = false;
        }

        private void OnDrawGizmos()
        {
            if (waypoints == null || waypoints.Count < 2)
                return;

            Gizmos.color = Color.yellow;
            for (int i = 0; i < waypoints.Count - 1; i++)
            {
                if (waypoints[i] == null || waypoints[i + 1] == null)
                    continue;

                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                Gizmos.DrawWireSphere(waypoints[i].position, 0.3f);
            }

            if (waypoints[^1] != null)
                Gizmos.DrawWireSphere(waypoints[^1].position, 0.3f);
        }
    }
}
