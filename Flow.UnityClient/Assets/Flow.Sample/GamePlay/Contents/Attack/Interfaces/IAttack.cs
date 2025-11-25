using Flow.Sample.Data.StaticData.Attack;

namespace Flow.Sample.GamePlay.Contents.Attack.Interfaces
{
    public interface IAttack
    {
        public AttackData Data { get; }
        
        public bool CanExecute();
        
        public void Execute(AttackContext context);
        
        public void Update(float deltaTime);
    }
}