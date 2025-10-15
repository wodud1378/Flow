using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;
using Flow.Sample.GamePlay;
using Flow.Sample.GamePlay.Systems;
using Flow.Sample.Logging;
using VContainer;
using VContainer.Unity;

namespace Flow.Sample.DI
{
    public class ApplicationScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Register logging
            builder.Register<IFlowLogger, FlowDefaultLogger>(Lifetime.Singleton);
            
            // Register entity container
            builder.Register<IEntityContainer, EntityContainer>(Lifetime.Singleton);
            
            // Register game systems
            builder.Register<TowerSystem>(Lifetime.Scoped);
            builder.Register<EnemySystem>(Lifetime.Scoped);
            builder.Register<ProjectileSystem>(Lifetime.Scoped);
            builder.Register<CombatSystem>(Lifetime.Scoped);
            builder.Register<PathSystem>(Lifetime.Scoped);
            
            // Note: WaveSystem, TowerDefenseGameManager, TowerPlacementController, and TowerDefenseUIController
            // are MonoBehaviours and should be added to GameObjects in the scene rather than registered in DI
        }
    }
}
