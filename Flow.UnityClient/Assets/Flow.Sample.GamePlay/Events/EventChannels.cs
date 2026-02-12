using VContainer;

namespace Flow.Sample.GamePlay.Events
{
    public class EventChannels
    {
        public readonly GameEvents Game;
        public readonly PlayerEvents Player;
        public readonly EnemyEvents Enemy;
        public readonly CombatEvents Combat;

        [Inject]
        public EventChannels(GameEvents game, PlayerEvents player, EnemyEvents enemy, CombatEvents combat)
        {
            Game = game;
            Player = player;
            Enemy = enemy;
            Combat = combat;
        }
    }
}