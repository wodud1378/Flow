namespace Flow.Sample.GamePlay.Contents.Attack.Interfaces
{
    public interface IAttackCondition
    {
        public bool Ready { get; }
        public void Reset();
        public void Update(float deltaTime);
    }
}