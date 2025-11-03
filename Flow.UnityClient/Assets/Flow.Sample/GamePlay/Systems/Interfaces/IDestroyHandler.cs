using Flow.Sample.Entities;

namespace Flow.Sample.GamePlay.Systems.Interfaces
{
    public interface IDestroyHandler
    {
        public void Destroy<T>(T entity) where T : BaseEntity;
    }
}