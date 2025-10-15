using System.Collections.Generic;
using UnityEngine;

namespace Flow.Sample.GamePlay.Systems
{
    public class PathSystem
    {
        private readonly List<Vector3> _waypoints = new();
        private readonly List<Transform> _waypointTransforms = new();
        
        public IReadOnlyList<Vector3> Waypoints => _waypoints;
        public int WaypointCount => _waypoints.Count;
        
        public void AddWaypoint(Vector3 position)
        {
            _waypoints.Add(position);
        }
        
        public void AddWaypoint(Transform transform)
        {
            _waypointTransforms.Add(transform);
            _waypoints.Add(transform.position);
        }
        
        public void ClearWaypoints()
        {
            _waypoints.Clear();
            _waypointTransforms.Clear();
        }
        
        public Vector3[] GetPath()
        {
            // Update positions from transforms if they exist
            for (int i = 0; i < _waypointTransforms.Count; i++)
            {
                if (_waypointTransforms[i] != null)
                {
                    _waypoints[i] = _waypointTransforms[i].position;
                }
            }
            
            return _waypoints.ToArray();
        }
        
        public Vector3 GetWaypoint(int index)
        {
            if (index < 0 || index >= _waypoints.Count)
                return Vector3.zero;
            
            // Update from transform if exists
            if (index < _waypointTransforms.Count && _waypointTransforms[index] != null)
            {
                _waypoints[index] = _waypointTransforms[index].position;
            }
            
            return _waypoints[index];
        }
        
        public float GetPathLength()
        {
            float length = 0f;
            for (int i = 0; i < _waypoints.Count - 1; i++)
            {
                length += Vector3.Distance(_waypoints[i], _waypoints[i + 1]);
            }
            return length;
        }
        
        public Vector3 GetPositionAlongPath(float normalizedPosition)
        {
            if (_waypoints.Count == 0) return Vector3.zero;
            if (_waypoints.Count == 1) return _waypoints[0];
            
            float totalLength = GetPathLength();
            float targetLength = totalLength * Mathf.Clamp01(normalizedPosition);
            float currentLength = 0f;
            
            for (int i = 0; i < _waypoints.Count - 1; i++)
            {
                float segmentLength = Vector3.Distance(_waypoints[i], _waypoints[i + 1]);
                
                if (currentLength + segmentLength >= targetLength)
                {
                    float t = (targetLength - currentLength) / segmentLength;
                    return Vector3.Lerp(_waypoints[i], _waypoints[i + 1], t);
                }
                
                currentLength += segmentLength;
            }
            
            return _waypoints[_waypoints.Count - 1];
        }
        
        public void DrawPath(Color color)
        {
            if (_waypoints.Count < 2) return;
            
            for (int i = 0; i < _waypoints.Count - 1; i++)
            {
                Debug.DrawLine(_waypoints[i], _waypoints[i + 1], color);
            }
        }
        
        public void DrawGizmos(Color color)
        {
            #if UNITY_EDITOR
            Gizmos.color = color;
            
            for (int i = 0; i < _waypoints.Count; i++)
            {
                Gizmos.DrawWireSphere(_waypoints[i], 0.5f);
                
                if (i < _waypoints.Count - 1)
                {
                    Gizmos.DrawLine(_waypoints[i], _waypoints[i + 1]);
                }
            }
            #endif
        }
    }
}
