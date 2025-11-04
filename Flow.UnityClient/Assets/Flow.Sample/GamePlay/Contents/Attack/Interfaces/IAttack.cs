namespace Flow.Sample.GamePlay.Contents.Attack.Interfaces
{
    public interface IAttack
    {
        public bool CanExecute();
        
        public void Execute(AttackContext context);
        
        public void Update(float deltaTime);
    }
}