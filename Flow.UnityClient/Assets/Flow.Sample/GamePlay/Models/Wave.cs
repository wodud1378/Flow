namespace Flow.Sample.GamePlay.Models
{
    public struct Wave
    {
        public int Number;
        public int EnemyKilled;
        public float TimeSinceStart;

        public Wave(int number, int enemyKilled, float timeSinceStart)
        {
            Number = number;
            EnemyKilled = enemyKilled;
            TimeSinceStart = timeSinceStart;
        }
    }
}