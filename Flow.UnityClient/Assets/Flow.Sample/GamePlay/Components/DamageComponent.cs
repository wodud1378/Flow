using UnityEngine;

namespace Flow.Sample.GamePlay.Components
{
    public class DamageComponent : MonoBehaviour, IComponent
    {
        [SerializeField] private float baseDamage = 10f;
        [SerializeField] private float damageMultiplier = 1f;
        [SerializeField] private DamageType damageType = DamageType.Physical;
        
        public float BaseDamage
        {
            get => baseDamage;
            set => baseDamage = value;
        }
        
        public float DamageMultiplier
        {
            get => damageMultiplier;
            set => damageMultiplier = value;
        }
        
        public DamageType DamageType
        {
            get => damageType;
            set => damageType = value;
        }
        
        public float CalculateDamage()
        {
            return baseDamage * damageMultiplier;
        }
    }
    
    public enum DamageType
    {
        Physical,
        Magic,
        Fire,
        Ice,
        Electric
    }
}
