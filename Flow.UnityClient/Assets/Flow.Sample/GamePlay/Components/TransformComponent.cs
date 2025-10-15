using UnityEngine;

namespace Flow.Sample.GamePlay.Components
{
    public class TransformComponent : MonoBehaviour, IComponent
    {
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
        
        public Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }
        
        public Vector3 Forward => transform.forward;
        public Vector3 Right => transform.right;
        public Vector3 Up => transform.up;
        
        public void LookAt(Vector3 target)
        {
            transform.LookAt(target);
        }
    }
}
