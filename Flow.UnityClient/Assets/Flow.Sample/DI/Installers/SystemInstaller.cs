using Flow.Sample.GamePlay.Configs;
using Flow.Sample.GamePlay.Systems;
using VContainer;

namespace Flow.Sample.DI.Installers
{
    public static class SystemInstaller
    {
        public static void InstallBasicSystems(this IContainerBuilder builder)
        {
            var lifetime = Lifetime.Singleton;

            builder.Register<PoolSystem>(lifetime);
            builder.Register<PlayerHpSystem>(lifetime);
            builder.Register<EnemySpawnSystem>(lifetime);
            builder.RegisterWithInterfaces<CleaningSystem>(lifetime);
            builder.RegisterWithInterfaces<UpdateContextSystem>(lifetime);
            builder.RegisterWithInterfaces<WaveSystem>(lifetime);
            builder.RegisterWithInterfaces<MoveOnPathSystem>(lifetime);
            builder.RegisterWithInterfaces<EnemyGoalDetectSystem>(lifetime);
        }

        public static void InstallCombatSystems(this IContainerBuilder builder, IConfig config)
        {
            var lifetime = Lifetime.Singleton;

            builder.RegisterInstance(config);
            builder.Register<DetectSystem>(lifetime);
            builder.RegisterWithInterfaces<CombatSystem>(lifetime);
        }

        private static void RegisterWithInterfaces<T>(this IContainerBuilder builder, Lifetime lifetime) =>
            builder.Register<T>(lifetime).AsImplementedInterfaces();
    }
}
