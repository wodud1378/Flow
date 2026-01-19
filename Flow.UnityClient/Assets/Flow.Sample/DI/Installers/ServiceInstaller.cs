using Flow.Core.Interfaces;
using Flow.Sample.GamePlay.Contents.Attack;
using Flow.Sample.GamePlay.Factories;
using Flow.Sample.GamePlay.Logics;
using Flow.Sample.GamePlay.Services;
using Flow.Sample.GamePlay.Systems;
using Flow.Sample.GamePlay.Systems.Interfaces;
using VContainer;

namespace Flow.Sample.DI.Installers
{
    public static class ServiceInstaller
    {
        public static void InstallServices(this IContainerBuilder builder)
        {
            var lifetime = Lifetime.Singleton;

            // Flow Services
            builder.Register<IInterruptionProvider, InterruptionProvider>(lifetime);
            builder.Register<ICompleteActionProvider, CompleteActionProvider>(lifetime);
            builder.Register<IUpdateGroupProvider, UpdateGroupProvider>(lifetime);
            builder.Register<IFlowServiceProvider, FlowServiceProvider>(lifetime);

            // Game Services
            builder.Register<TowerBuildService>(lifetime);
            builder.Register<ITowerInstallValidator, TowerInstallValidator>(lifetime);

            // Factories
            builder.Register<AttackFactory>(lifetime);
            builder.Register<DetectParamsProvider>(lifetime);

            // Logics
            builder.Register<DamageCalculator>(lifetime);
        }
    }
}
