using Flow.Sample.GamePlay.Entities;
using Flow.Sample.GamePlay.Systems.Base;
using R3;
using UnityEngine;
using VContainer;

namespace Flow.Sample.GamePlay.Input
{
    public class GameInputSystem : BaseUpdateSystem
    {
        private readonly InputEvents _events;
        private readonly Camera _camera;

        private readonly Collider2D[] _hitBuffer = new Collider2D[8];

        [Inject]
        public GameInputSystem(InputEvents events)
        {
            _events = events;
            _camera = Camera.main;
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (_camera == null)
                return;

            var mousePosition = UnityEngine.Input.mousePosition;
            var worldPosition = (Vector2)_camera.ScreenToWorldPoint(mousePosition);

            _events.PointerMovedStream.OnNext(worldPosition);

            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                HandleLeftClick(worldPosition);
            }

            if (UnityEngine.Input.GetMouseButtonDown(1) || UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                _events.PlacementCancelledStream.OnNext(Unit.Default);
            }
        }

        private void HandleLeftClick(Vector2 worldPosition)
        {
            // Check if clicked on existing tower
            var hitCount = Physics2D.OverlapPointNonAlloc(worldPosition, _hitBuffer);
            for (int i = 0; i < hitCount; i++)
            {
                var tower = _hitBuffer[i].GetComponent<TowerEntity>();
                if (tower != null && tower.IsValid)
                {
                    _events.TowerClickedStream.OnNext(tower);
                    return;
                }
            }

            // Otherwise, confirm placement at this position
            _events.PlacementConfirmedStream.OnNext(worldPosition);
        }
    }
}
