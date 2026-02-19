using Flow.Sample.GamePlay.Contents.Attack.Interfaces;
using Flow.Sample.GamePlay.Services.Interfaces;
using Flow.Sample.View.Contents.Attack;
using Flow.Sample.View.UI.Tower;
using UnityEngine;
using VContainer;

namespace Flow.Sample.View.DI
{
    public class ViewInstaller : MonoBehaviour
    {
        [SerializeField] private TowerSelectionView towerSelectionView;
        
        public void InstallViewServices(IContainerBuilder builder)
        {
            var lifetime = Lifetime.Singleton;

            builder.Register<IAttackViewSyncProvider, AttackVIewSyncProvider>(lifetime);
            builder.RegisterInstance<ITowerSelector>(towerSelectionView);
        }
    }
}
