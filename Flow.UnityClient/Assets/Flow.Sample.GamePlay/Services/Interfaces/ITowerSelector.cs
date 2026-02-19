using Flow.Sample.Data.StaticData.Tower;
using R3;

namespace Flow.Sample.GamePlay.Services.Interfaces
{
    public interface ITowerSelector
    {
        public ReadOnlyReactiveProperty<TowerData> Selected { get; }
    }
}