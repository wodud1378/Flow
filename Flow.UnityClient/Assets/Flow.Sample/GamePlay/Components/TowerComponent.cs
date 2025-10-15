using UnityEngine;

namespace Flow.Sample.GamePlay.Components
{
    public class TowerComponent : MonoBehaviour, IComponent
    {
        [Header("Tower Settings")]
        [SerializeField] private TowerType towerType = TowerType.Basic;
        [SerializeField] private int upgradeLevel = 1;
        [SerializeField] private int maxUpgradeLevel = 3;
        
        [Header("Attack Settings")]
        [SerializeField] private float attackSpeed = 1f;
        [SerializeField] private float attackRange = 10f;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform firePoint;
        
        [Header("Cost")]
        [SerializeField] private int buildCost = 100;
        [SerializeField] private int upgradeCost = 50;
        [SerializeField] private int sellValue = 75;
        
        private float _lastAttackTime;
        
        public TowerType TowerType => towerType;
        public int UpgradeLevel => upgradeLevel;
        public int MaxUpgradeLevel => maxUpgradeLevel;
        public bool CanUpgrade => upgradeLevel < maxUpgradeLevel;
        
        public float AttackSpeed
        {
            get => attackSpeed;
            set => attackSpeed = value;
        }
        
        public float AttackRange
        {
            get => attackRange;
            set => attackRange = value;
        }
        
        public GameObject ProjectilePrefab => projectilePrefab;
        public Transform FirePoint => firePoint;
        
        public int BuildCost => buildCost;
        public int UpgradeCost => upgradeCost * upgradeLevel;
        public int SellValue => sellValue * upgradeLevel;
        
        public bool CanAttack()
        {
            return Time.time - _lastAttackTime >= 1f / attackSpeed;
        }
        
        public void OnAttack()
        {
            _lastAttackTime = Time.time;
        }
        
        public void Upgrade()
        {
            if (!CanUpgrade) return;
            
            upgradeLevel++;
            attackSpeed *= 1.2f;
            
            var damage = GetComponent<DamageComponent>();
            if (damage != null)
            {
                damage.DamageMultiplier *= 1.5f;
            }
        }
    }
    
    public enum TowerType
    {
        Basic,
        Cannon,
        Laser,
        Freeze,
        Missile
    }
}
