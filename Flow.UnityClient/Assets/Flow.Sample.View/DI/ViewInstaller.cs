using Flow.Sample.GamePlay.Contents.Attack.Interfaces;
using Flow.Sample.View.Contents.Attack;
using VContainer;

namespace Flow.Sample.View.DI
{
    public static class ViewInstaller
    {
        public static void InstallViewServices(this IContainerBuilder builder)
        {
            var lifetime = Lifetime.Singleton;

            builder.Register<IAttackViewSyncProvider, AttackVIewSyncProvider>(lifetime);
        }
    }
}
