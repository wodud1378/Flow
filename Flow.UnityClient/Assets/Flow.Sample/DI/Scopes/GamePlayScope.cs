using Flow.Core.Interfaces;
using Flow.Sample.DI.Configs;
using Flow.Sample.DI.Installers;
using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;
using Flow.Sample.GamePlay;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Flow.Sample.DI.Scopes
{
    public class GamePlayScope : LifetimeScope
    {
        [SerializeField] private BufferConfig bufferConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IGameContext, GameContext>(Lifetime.Singleton);
            builder.Register<IEntityContainer, EntityContainer>(Lifetime.Singleton);
            
            builder.InstallBasicSystems();
            builder.InstallCombatSystems(bufferConfig);
        }
    }
}