using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Flow.Sample.Entities
{
    public abstract class BaseEntity : MonoBehaviour
    {
        public int Id { get; private set; }
        public bool IsValid { get; private set; }
        
        public bool DestroyTriggered => _lifeTimeCt.IsCancellationRequested;

        private CancellationToken _lifeTimeCt;
        
        private void Awake()
        {
            _lifeTimeCt = this.GetCancellationTokenOnDestroy();
        }

        public virtual void Initialize(int id)
        {
            Id = id;
            IsValid = true;
        }

        public virtual void Invalidate()
        {
            IsValid = false;
        }
    }
}