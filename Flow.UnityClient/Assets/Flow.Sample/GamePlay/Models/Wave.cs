namespace Flow.Sample.GamePlay.Models
{
    public readonly struct Wave
    {
        public readonly int Number;
        public readonly int EnemyKilled;
        public readonly float TimeSinceStart;

        public Wave(int number, int enemyKilled, float timeSinceStart)
        {
            Number = number;
            EnemyKilled = enemyKilled;
            TimeSinceStart = timeSinceStart;
        }

        public Wave Copy(
            int? number = null, 
            int? enemyKilled = null, 
            float? timeSinceStart = null) =>
            new(
                number ?? Number,
                enemyKilled ?? EnemyKilled,
                timeSinceStart ?? TimeSinceStart
            );
    }
}