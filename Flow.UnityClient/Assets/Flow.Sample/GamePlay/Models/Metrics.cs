namespace Flow.Sample.GamePlay.Models
{
    public struct Metrics
    {
        public int Hp;
        public int Gold;
        public int Score;

        public Metrics(int hp, int gold, int score)
        {
            Gold = gold;
            Score = score;
            Hp = hp;
        }

        public Metrics Copy(
            int? hp = null,
            int? gold = null,
            int? score = null) =>
            new(
                hp ?? Hp,
                gold ?? Gold,
                score ?? Score
            );
    }
}