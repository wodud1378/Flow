using Flow.Sample.Entities;
using Flow.Sample.Entities.Interfaces;
using Flow.Sample.GamePlay.Systems;
using Flow.Sample.GamePlay.Systems.Base;
using System.Collections.Generic;

namespace Flow.Sample.DI.Installers
{
    /// <summary>
    /// Factory class for creating game systems
    /// This is used when not using dependency injection container
    /// </summary>
    public static class GamePlaySystemInstaller
    {
        public static List<BaseSystem> CreateGameSystems(IEntityContainer entityContainer)
        {
            var systems = new List<BaseSystem>();
            
            // Create all game systems
            systems.Add(new TowerSystem(entityContainer));
            systems.Add(new EnemySystem(entityContainer));
            systems.Add(new ProjectileSystem(entityContainer));
            systems.Add(new CombatSystem(entityContainer));
            
            return systems;
        }
        
        public static IEntityContainer CreateEntityContainer()
        {
            return new EntityContainer();
        }
    }
}
