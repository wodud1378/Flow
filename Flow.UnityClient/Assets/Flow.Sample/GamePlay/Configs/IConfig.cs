namespace Flow.Sample.GamePlay.Configs
{
    public interface IConfig
    {
        public int DetectBufferSize { get; }
        public int UpdateEntitySystemBufferSize { get; }
        
        public int EnemySpawnLimitPerFrame { get; }
    }
}