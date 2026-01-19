using Flow.Sample.Data.StaticData.Tower;
using Flow.Sample.GamePlay.Entities;
using R3;

namespace Flow.Sample.GamePlay.Input
{
    public class TowerPlacementEvents
    {
        public Observable<TowerData> OnPlacementStarted => PlacementStartedStream;
        public Observable<Unit> OnPlacementCancelled => PlacementCancelledStream;
        public Observable<PlacementPreview> OnPreviewUpdated => PreviewUpdatedStream;
        public Observable<TowerEntity> OnTowerPlaced => TowerPlacedStream;
        public Observable<string> OnPlacementFailed => PlacementFailedStream;

        internal Subject<TowerData> PlacementStartedStream { get; } = new();
        internal Subject<Unit> PlacementCancelledStream { get; } = new();
        internal Subject<PlacementPreview> PreviewUpdatedStream { get; } = new();
        internal Subject<TowerEntity> TowerPlacedStream { get; } = new();
        internal Subject<string> PlacementFailedStream { get; } = new();
    }
}
