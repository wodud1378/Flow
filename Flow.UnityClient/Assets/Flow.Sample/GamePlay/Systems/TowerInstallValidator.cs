using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Entities;
using Flow.Sample.GamePlay.Entities.Interfaces;
using Flow.Sample.GamePlay.Systems.Interfaces;
using UnityEngine;
using VContainer;

namespace Flow.Sample.GamePlay.Systems
{
    public class TowerInstallValidator : ITowerInstallValidator
    {
        private readonly IEntityContainer _entityContainer;
        private readonly ResourceSystem _resourceSystem;

        private readonly Collider2D[] _overlapBuffer = new Collider2D[16];

        [Inject]
        public TowerInstallValidator(IEntityContainer entityContainer, ResourceSystem resourceSystem)
        {
            _entityContainer = entityContainer;
            _resourceSystem = resourceSystem;
        }

        public bool CanInstall(Vector2 position, float radius)
        {
            // Check overlap with existing towers
            foreach (var kvp in _entityContainer.Entities)
            {
                if (kvp.Value is not TowerEntity tower)
                    continue;

                if (!tower.TryGetComponent<SizeComponent>(out var size))
                    continue;

                var distance = Vector2.Distance(position, tower.transform.position);
                var minDistance = radius + size.Radius;

                if (distance < minDistance)
                    return false;
            }

            // Check if position is on valid terrain (using Physics2D)
            var hitCount = Physics2D.OverlapCircleNonAlloc(position, radius, _overlapBuffer);
            for (int i = 0; i < hitCount; i++)
            {
                var collider = _overlapBuffer[i];
                if (collider.CompareTag("InvalidPlacement"))
                    return false;
            }

            return true;
        }

        public bool HasEnoughGold(int cost)
        {
            return _resourceSystem.CanAfford(cost);
        }
    }
}
