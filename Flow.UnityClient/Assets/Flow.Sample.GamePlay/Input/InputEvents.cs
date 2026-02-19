using Flow.Sample.GamePlay.Entities;
using R3;
using UnityEngine;

namespace Flow.Sample.GamePlay.Input
{
    public class InputEvents
    {
        public Observable<Vector2> OnPlacementConfirmed => PlacementConfirmedStream;
        public Observable<Unit> OnPlacementCancelled => PlacementCancelledStream;
        public Observable<Vector2> OnPointerMoved => PointerMovedStream;
        public Observable<TowerEntity> OnTowerClicked => TowerClickedStream;

        internal Subject<Vector2> PlacementConfirmedStream { get; } = new();
        internal Subject<Unit> PlacementCancelledStream { get; } = new();
        internal Subject<Vector2> PointerMovedStream { get; } = new();
        internal Subject<TowerEntity> TowerClickedStream { get; } = new();
    }
}
