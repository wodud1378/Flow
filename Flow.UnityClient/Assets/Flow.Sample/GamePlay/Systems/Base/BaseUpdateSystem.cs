using Flow.Sample.GamePlay.Events;

namespace Flow.Sample.GamePlay.Systems.Base
{
    public abstract class BaseUpdateSystem
    {
        public bool Enabled { get; set; }
        
        private bool _previousEnabled;

        public void Update(float deltaTime)
        {
            if (Enabled)
            {
                if (_previousEnabled == false)
                {
                    _previousEnabled = true;
                    OnStartRunning();
                }

                OnUpdate(deltaTime);
            }
            else
            {
                _previousEnabled = false;
                OnStopRunning();
            }
        }

        protected virtual void OnStopRunning()
        {
        }

        protected virtual void OnStartRunning()
        {
        }

        protected abstract void OnUpdate(float deltaTime);
    }
}