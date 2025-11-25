using Flow.Sample.GamePlay.Components.Interfaces;
using Flow.Sample.GamePlay.Entities;
using UnityEngine;

namespace Flow.Sample.GamePlay.Components
{
    public class SizeComponent : MonoBehaviour, IComponent
    {
        [field:SerializeField] public float Radius { get; private set; }
        [field:SerializeField] public BaseEntity Owner { get; private set; }

        private void OnValidate()
        {
            if(Owner == null)
                Owner = GetComponent<BaseEntity>();
        }
    }
}