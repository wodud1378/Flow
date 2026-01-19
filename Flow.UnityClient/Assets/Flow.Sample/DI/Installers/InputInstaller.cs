using Flow.Sample.GamePlay.Input;
using VContainer;

namespace Flow.Sample.DI.Installers
{
    public static class InputInstaller
    {
        public static void InstallInputSystems(this IContainerBuilder builder)
        {
            var lifetime = Lifetime.Singleton;

            builder.Register<GameInputSystem>(lifetime);
            builder.Register<TowerPlacementController>(lifetime);
        }
    }
}
