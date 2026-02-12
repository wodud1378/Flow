using Flow.Sample.GamePlay.Entities;

namespace Flow.Sample.GamePlay.Components.Interfaces
{
    public interface IComponent
    {
        public BaseEntity Owner { get; }
    }
}