namespace Flow.Sample.GamePlay.Configs
{
    public interface IBufferConfig
    {
        public int DetectBufferSize { get; }
        public int UpdateEntitySystemBufferSize { get; }
    }
}