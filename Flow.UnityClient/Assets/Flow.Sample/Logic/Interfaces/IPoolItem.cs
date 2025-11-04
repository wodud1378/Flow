using UnityEngine;

namespace Flow.Sample.Logic.Interfaces
{
    public interface IPoolItem
    {
        public GameObject Origin { get; }
        public bool IsActive { get; }
        
        public void Activate();
        public void Deactivate();
    }
}