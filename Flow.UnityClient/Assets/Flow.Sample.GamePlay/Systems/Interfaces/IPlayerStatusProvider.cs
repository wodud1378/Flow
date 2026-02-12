namespace Flow.Sample.GamePlay.Systems.Interfaces
{
    public interface IPlayerStatusProvider
    {
        public float Hp { get; }
        public float HpRecoveryRate { get; }
        public float HpRecoveryTime { get; }
    }
}