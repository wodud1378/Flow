using Flow.Sample.GamePlay.Entities;
using Flow.Sample.GamePlay.Entities.Interfaces;
using Flow.Sample.GamePlay.Systems;
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
        }
    }
}
