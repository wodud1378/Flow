using Flow.Sample.GamePlay.Entities;
using Flow.Sample.GamePlay.Entities.Interfaces;
using Flow.Sample.GamePlay.Providers;
using Flow.Sample.GamePlay.Systems;
using Flow.Sample.GamePlay.Systems.Interfaces;
using VContainer;

namespace Flow.Sample.DI.Installers
{
    public static class EntityInstaller
    {
        public static void InstallEntitySystems(this IContainerBuilder builder)
        {
            var lifetime = Lifetime.Singleton;

            builder.Register<IEntityContainer, EntityContainer>(lifetime);
            builder.Register<EntityIdGenerator>(lifetime);
            builder.Register<EntitySystem>(lifetime);

            // Providers
            builder.Register<IEnemyWaveProvider, EnemyWaveProvider>(lifetime);
            builder.Register<IMovePathProvider, MovePathProvider>(lifetime);
            builder.Register<IPlayerStatusProvider, PlayerStatusProvider>(lifetime);
        }
    }
}
