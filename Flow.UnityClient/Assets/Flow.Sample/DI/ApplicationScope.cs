using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;
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
        }
    }
}
