using System;
using Flow.Sample.Data.StaticData.Tower;
using Flow.Sample.GamePlay.Events;
using Flow.Sample.GamePlay.Services;
using Flow.Sample.GamePlay.Services.Interfaces;
using Flow.Sample.GamePlay.Systems.Base;
using R3;
using UnityEngine;
using VContainer;

namespace Flow.Sample.GamePlay.Input
{
    public class TowerPlacementController : BaseUpdateSystem
    {
        private readonly InputEvents _inputEvents;
        private readonly TowerBuildService _buildService;
        private readonly TowerPlacementEvents _placementEvents;

        private readonly IDisposable _subscriptions;

        private TowerData _selectedTower;
        private Vector2 _currentPosition;
        private bool _isPlacing;

        public bool IsPlacing => _isPlacing;
        public TowerData SelectedTower => _selectedTower;

        [Inject]
        public TowerPlacementController(
            ITowerSelector towerSelector,
            InputEvents inputEvents,
            TowerBuildService buildService,
            TowerPlacementEvents placementEvents)
        {
            _inputEvents = inputEvents;
            _buildService = buildService;
            _placementEvents = placementEvents;

            _subscriptions = Disposable.Combine(
                towerSelector.Selected.Subscribe(OnTowerSelected),
                inputEvents.OnPlacementConfirmed.Subscribe(OnPlacementConfirmed),
                inputEvents.OnPlacementCancelled.Subscribe(_ => CancelPlacement()),
                inputEvents.OnPointerMoved.Subscribe(OnPointerMoved)
            );
        }

        ~TowerPlacementController()
        {
            _subscriptions?.Dispose();
        }

        private void SelectTower(TowerData data)
        {
            _selectedTower = data;
            _isPlacing = true;
            _placementEvents.PlacementStartedStream.OnNext(data);
        }

        private void CancelPlacement()
        {
            if (!_isPlacing)
                return;

            _selectedTower = null;
            _isPlacing = false;
            _placementEvents.PlacementCancelledStream.OnNext(Unit.Default);
        }

        private void OnTowerSelected(TowerData data)
        {
            SelectTower(data);
        }

        private void OnPointerMoved(Vector2 position)
        {
            if (!_isPlacing)
                return;

            _currentPosition = position;
            var canPlace = _buildService.CanBuild(_selectedTower, position);
            _placementEvents.PreviewUpdatedStream.OnNext(new PlacementPreview(position, canPlace, _selectedTower));
        }

        private void OnPlacementConfirmed(Vector2 position)
        {
            if (!_isPlacing || _selectedTower == null)
                return;

            if (!_buildService.CanBuild(_selectedTower, position))
            {
                _placementEvents.PlacementFailedStream.OnNext("Cannot place tower here");
                return;
            }

            try
            {
                var tower = _buildService.FromData(_selectedTower, position).Build();
                _placementEvents.TowerPlacedStream.OnNext(tower);

                // Stay in placement mode for multi-placement
                // CancelPlacement();
            }
            catch (Exception e)
            {
                _placementEvents.PlacementFailedStream.OnNext(e.Message);
            }
        }

        protected override void OnUpdate(float deltaTime)
        {
            // Input handling is event-driven
        }
    }

    public readonly struct PlacementPreview
    {
        public readonly Vector2 Position;
        public readonly bool CanPlace;
        public readonly TowerData TowerData;

        public PlacementPreview(Vector2 position, bool canPlace, TowerData towerData)
        {
            Position = position;
            CanPlace = canPlace;
            TowerData = towerData;
        }
    }
}
