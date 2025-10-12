namespace Flow.Sample.Logging
{
    public interface IFlowLogger
    {
        public void Log(string message);
        public void LogWarning(string message);
        public void LogError(string message);
    }
}