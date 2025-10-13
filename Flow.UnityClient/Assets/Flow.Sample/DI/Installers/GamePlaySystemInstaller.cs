using Flow.Sample.GamePlay.Systems;
using VContainer;

namespace Flow.Sample.DI.Installers
{
    public static class GamePlaySystemInstaller
    {
        public static void Install(ContainerBuilder builder)
        {
            builder.Register<CombatSystem>(Lifetime.Scoped);
            builder.Register<EnemySystem>(Lifetime.Scoped);
            builder.Register<PathSystem>(Lifetime.Scoped);
            builder.Register<TowerSystem>(Lifetime.Scoped);
            builder.Register<WaveSystem>(Lifetime.Scoped);
        }
    }
}