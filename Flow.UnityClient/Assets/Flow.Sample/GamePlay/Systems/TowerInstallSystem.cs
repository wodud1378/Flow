using Flow.Sample.GamePlay.Entities;
using UnityEngine;
using VContainer;

namespace Flow.Sample.GamePlay.Systems
{
    public class TowerInstallSystem
    {
        private readonly EntitySystem _entitySystem;

        [Inject]
        public TowerInstallSystem(EntitySystem entitySystem)
        {
            _entitySystem = entitySystem;
        }
        
        private void Create(TowerEntity prefab, Vector2 position)
        {
            var tower = _entitySystem.New(prefab);
            tower.transform.position = position;
        }
    }
}