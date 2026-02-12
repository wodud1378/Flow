using Flow.Sample.GamePlay.Events;
using Flow.Sample.GamePlay.Models;
using Flow.Sample.GamePlay.Systems.Base;
using VContainer;

namespace Flow.Sample.GamePlay.Systems
{
    public class ResourceSystem : BaseUpdateSystem
    {
        private readonly PlayerEvents _events;

        private int _gold;
        private int _score;
        private int _initialGold;

        public int Gold => _gold;
        public int Score => _score;

        [Inject]
        public ResourceSystem(PlayerEvents events)
        {
            _events = events;
        }

        public void Initialize(int initialGold)
        {
            _initialGold = initialGold;
            _gold = initialGold;
            _score = 0;
            NotifyMetricsChanged();
        }

        public bool CanAfford(int cost) => _gold >= cost;

        public bool TrySpendGold(int amount)
        {
            if (!CanAfford(amount))
                return false;

            _gold -= amount;
            NotifyMetricsChanged();
            return true;
        }

        public void AddGold(int amount)
        {
            _gold += amount;
            NotifyMetricsChanged();
        }

        public void AddScore(int amount)
        {
            _score += amount;
            NotifyMetricsChanged();
        }

        private void NotifyMetricsChanged()
        {
            var metrics = new Metrics(0, _gold, _score);
            _events.MeticsUpdatedStream.OnNext(metrics);
        }

        protected override void OnStartRunning()
        {
            if (_gold == 0)
                Initialize(100);
        }

        protected override void OnUpdate(float deltaTime)
        {
            // Resource system is event-driven
        }
    }
}
