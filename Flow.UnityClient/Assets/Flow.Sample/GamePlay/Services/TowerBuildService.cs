using System;
using Flow.Sample.Data.StaticData.Attack;
using Flow.Sample.Data.StaticData.Tower;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Entities;
using Flow.Sample.GamePlay.Factories;
using Flow.Sample.GamePlay.Systems;
using Flow.Sample.GamePlay.Systems.Interfaces;
using UnityEngine;
using VContainer;

namespace Flow.Sample.GamePlay.Services
{
    public class TowerBuildService
    {
        private readonly EntitySystem _entitySystem;
        private readonly AttackFactory _attackFactory;
        private readonly ResourceSystem _resourceSystem;
        private readonly ITowerInstallValidator _validator;

        private TowerEntity _instance;
        private bool _hasInstance;

        [Inject]
        public TowerBuildService(
            EntitySystem entitySystem,
            AttackFactory attackFactory,
            ResourceSystem resourceSystem,
            ITowerInstallValidator validator)
        {
            _entitySystem = entitySystem;
            _attackFactory = attackFactory;
            _resourceSystem = resourceSystem;
            _validator = validator;
        }

        public bool CanBuild(TowerData data, Vector2 position)
        {
            return _validator.HasEnoughGold(data.buildCost) &&
                   _validator.CanInstall(position, data.radius);
        }

        public TowerBuildService FromData(TowerData data, Vector2 position)
        {
            if (!CanBuild(data, position))
                throw new InvalidOperationException("Cannot build tower at this position or not enough gold.");

            if (!ValidateTowerPrefab(data.prefab, out var prefab))
                throw new InvalidOperationException($"TowerData.prefab does not have TowerEntity component: {data.towerName}");

            _resourceSystem.TrySpendGold(data.buildCost);

            _instance = _entitySystem.New(prefab);
            _hasInstance = _instance != null;

            if (_hasInstance)
            {
                _instance.transform.position = position;

                if (_instance.TryGetComponent<SizeComponent>(out var size))
                    size.Radius = data.radius;

                if (data.attackData != null)
                    WithAttack(data.attackData);
            }

            return this;
        }

        private bool ValidateTowerPrefab(GameObject prefabObject, out TowerEntity prefab)
        {
            prefab = null;

            if (prefabObject == null)
                return false;

            return prefabObject.TryGetComponent(out prefab);
        }

        public TowerBuildService New(TowerEntity prefab)
        {
            _instance = _entitySystem.New(prefab);
            _hasInstance = _instance != null;
            return this;
        }

        public TowerBuildService OnPosition(Vector2 position)
        {
            ThrowIfInstanceNotSet();

            _instance.transform.position = position;
            return this;
        }

        public TowerBuildService WithAttack(AttackData data)
        {
            ThrowIfInstanceNotSet();

            var combatant = _instance.Combatant;
            var attack = _attackFactory.Create(combatant, data);
            combatant.AddAttack(attack);
            return this;
        }

        public TowerEntity Build()
        {
            ThrowIfInstanceNotSet();

            var result = _instance;
            _instance = null;
            _hasInstance = false;
            return result;
        }

        public void SellTower(TowerEntity tower, int sellValue)
        {
            if (tower == null || !tower.IsValid)
                return;

            _resourceSystem.AddGold(sellValue);
            _entitySystem.Invalidate(tower);
        }

        private void ThrowIfInstanceNotSet()
        {
            if (!_hasInstance)
                throw new Exception("Tower instance is not set yet.");
        }
    }
}
