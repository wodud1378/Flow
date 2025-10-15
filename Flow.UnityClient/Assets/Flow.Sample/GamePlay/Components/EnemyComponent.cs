using UnityEngine;

namespace Flow.Sample.GamePlay.Components
{
    public class EnemyComponent : MonoBehaviour, IComponent
    {
        [Header("Enemy Settings")]
        [SerializeField] private EnemyType enemyType = EnemyType.Basic;
        [SerializeField] private int rewardGold = 10;
        [SerializeField] private int damageToBase = 1;
        
        [Header("Path Settings")]
        [SerializeField] private int currentWaypoint = 0;
        [SerializeField] private Vector3[] pathWaypoints;
        
        public EnemyType EnemyType => enemyType;
        public int RewardGold => rewardGold;
        public int DamageToBase => damageToBase;
        
        public int CurrentWaypoint
        {
            get => currentWaypoint;
            set => currentWaypoint = value;
        }
        
        public Vector3[] PathWaypoints
        {
            get => pathWaypoints;
            set => pathWaypoints = value;
        }
        
        public bool HasReachedEnd => currentWaypoint >= pathWaypoints.Length;
        
        public Vector3 GetCurrentTargetPosition()
        {
            if (HasReachedEnd || pathWaypoints == null || pathWaypoints.Length == 0)
                return transform.position;
            
            return pathWaypoints[currentWaypoint];
        }
        
        public void MoveToNextWaypoint()
        {
            if (!HasReachedEnd)
            {
                currentWaypoint++;
            }
        }
        
        public void SetPath(Vector3[] path)
        {
            pathWaypoints = path;
            currentWaypoint = 0;
        }
    }
    
    public enum EnemyType
    {
        Basic,
        Fast,
        Tank,
        Flying,
        Boss
    }
}
