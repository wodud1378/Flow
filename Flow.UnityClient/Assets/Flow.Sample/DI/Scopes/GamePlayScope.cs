using Flow.Core.Interfaces;
using Flow.Sample.DI.Configs;
using Flow.Sample.DI.Installers;
using Flow.Sample.GamePlay;
using Flow.Sample.View.DI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Flow.Sample.DI.Scopes
{
    public class GamePlayScope : LifetimeScope
    {
        [SerializeField] private Config config;
        [SerializeField] private ViewInstaller viewInstaller;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<GameContext>(Lifetime.Singleton).As<IGameContext, GameContext>();

            builder.InstallEntitySystems();
            builder.InstallEventChannels();
            builder.InstallServices();
            builder.InstallBasicSystems();
            builder.InstallCombatSystems(config);
            builder.InstallInputSystems();
            
            viewInstaller.InstallViewServices(builder);
        }
    }
}
