using Flow.Sample.Entities;
using Flow.Sample.GamePlay.Components.Interfaces;
using UnityEngine;

namespace Flow.Sample.GamePlay.Components
{
    public class MoveOnPathComponent : MonoBehaviour, IComponent
    {
        [field:SerializeField] public float Speed { get; set; }
        
        public BaseEntity Owner { get; }
    }
}