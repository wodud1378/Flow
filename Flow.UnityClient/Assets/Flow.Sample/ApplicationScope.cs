using Flow.Sample.Logging;
using VContainer;
using VContainer.Unity;

namespace Flow.Sample
{
    public class ApplicationScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IFlowLogger, FlowDefaultLogger>(Lifetime.Singleton);
        }
    }
}