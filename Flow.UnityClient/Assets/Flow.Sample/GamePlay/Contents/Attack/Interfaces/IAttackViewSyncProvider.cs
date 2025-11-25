using Flow.Sample.Data.StaticData.Attack;

namespace Flow.Sample.GamePlay.Contents.Attack.Interfaces
{
    public interface IAttackViewSyncProvider
    {
        public IAttackViewSync Provide(AttackData data);
    }
}