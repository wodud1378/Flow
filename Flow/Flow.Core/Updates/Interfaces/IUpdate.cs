namespace Flow.Core.Updates.Interfaces
{
    public interface IUpdate
    {
        public bool Enabled { get; set; }
        
        public void Update(float deltaTime);
    }
}