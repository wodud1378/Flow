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
            
            builder.Register<EntityPoolSystem>(lifetime);
            builder.Register<ComponentCacheSystem>(lifetime);
            builder.Register<CleaningSystem>(lifetime);
            builder.Register<UpdateContextSystem>(lifetime);
            builder.Register<WaveSystem>(lifetime);
        }
        
        public static void InstallCombatSystems(this IContainerBuilder builder, IBufferConfig bufferConfig)
        {
            var lifetime = Lifetime.Singleton;

            builder.RegisterInstance(bufferConfig);
            builder.Register<DetectSystem>(lifetime);
            builder.Register<CombatSystem>(lifetime);
        }
    }
}