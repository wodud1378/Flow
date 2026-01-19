using Flow.Sample.GamePlay.Events;
using Flow.Sample.GamePlay.Input;
using Flow.Sample.GamePlay.States;
using VContainer;

namespace Flow.Sample.DI.Installers
{
    public static class EventChannelInstaller
    {
        public static void InstallEventChannels(this IContainerBuilder builder)
        {
            var lifetime = Lifetime.Singleton;

            builder.Register<GameEvents>(lifetime);
            builder.Register<PlayerEvents>(lifetime);
            builder.Register<EnemyEvents>(lifetime);
            builder.Register<CombatEvents>(lifetime);
            builder.Register<EventChannels>(lifetime);

            // New events
            builder.Register<GameStateEvents>(lifetime);
            builder.Register<InputEvents>(lifetime);
            builder.Register<TowerPlacementEvents>(lifetime);
        }
    }
}