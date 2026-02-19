namespace Flow.Sample.GamePlay.Events.Models
{
    public readonly struct HpChanged
    {
        public readonly float Current;
        public readonly float Previous;
        public readonly float Max;
        
        public HpChanged(float current, float previous, float max)
        {
            Current = current;
            Previous = previous;
            Max = max;
        }
    }
}