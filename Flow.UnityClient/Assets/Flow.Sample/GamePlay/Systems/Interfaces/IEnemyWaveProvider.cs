using Flow.Sample.GamePlay.Systems.Models;

namespace Flow.Sample.GamePlay.Systems.Interfaces
{
    public interface IEnemyWaveProvider
    {
        public bool IsLastWave();
        public EnemyWave Next();
    }
}