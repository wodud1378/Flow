namespace Flow.Sample.GamePlay.Models
{
    public readonly struct Metrics
    {
        public readonly int Gold;
        public readonly int Lives;
        public readonly int Score;

        public Metrics(int gold, int lives, int score)
        {
            Gold = gold;
            Lives = lives;
            Score = score;
        }

        public Metrics Copy(
            int? gold = null,
            int? lives = null,
            int? score = null) =>
            new(
                gold ?? Gold,
                lives ?? Lives,
                score ?? Score
            );
    }
}