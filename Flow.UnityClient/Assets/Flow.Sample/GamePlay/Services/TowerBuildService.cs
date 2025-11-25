using System;
using Flow.Sample.Data.StaticData.Attack;
using Flow.Sample.GamePlay.Entities;
using Flow.Sample.GamePlay.Factories;
using Flow.Sample.GamePlay.Systems;
using UnityEngine;
using VContainer;

namespace Flow.Sample.GamePlay.Services
{
    public class TowerBuildService
    {
        private readonly EntitySystem _entitySystem;
        private readonly AttackFactory _attackFactory;

        private TowerEntity _instance;
        private bool _hasInstance;
        
        [Inject]
        public TowerBuildService(EntitySystem entitySystem, AttackFactory attackFactory)
        {
            _entitySystem = entitySystem;
            _attackFactory = attackFactory;
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
        
        private void ThrowIfInstanceNotSet()
        {
            if (!_hasInstance)
                throw new Exception("Tower instance is not set yet.");
        }
    }
}