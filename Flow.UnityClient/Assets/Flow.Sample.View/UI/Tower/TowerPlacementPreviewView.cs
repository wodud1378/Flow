using System;
using Flow.Sample.GamePlay.Input;
using R3;
using UnityEngine;
using VContainer;

namespace Flow.Sample.View.UI.Tower
{
    public class TowerPlacementPreviewView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer previewSprite;
        [SerializeField] private SpriteRenderer rangeIndicator;
        [SerializeField] private Color validColor = new Color(0, 1, 0, 0.5f);
        [SerializeField] private Color invalidColor = new Color(1, 0, 0, 0.5f);

        private IDisposable _subscriptions;

        [Inject]
        public void Initialize(TowerPlacementEvents events)
        {
            _subscriptions = Disposable.Combine(
                events.OnPlacementStarted.Subscribe(_ => Show()),
                events.OnPlacementCancelled.Subscribe(_ => Hide()),
                events.OnPreviewUpdated.Subscribe(OnPreviewUpdated),
                events.OnTowerPlaced.Subscribe(_ => { }) // Keep showing for multi-placement
            );

            Hide();
        }

        private void OnDestroy()
        {
            _subscriptions?.Dispose();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnPreviewUpdated(PlacementPreview preview)
        {
            transform.position = preview.Position;

            var color = preview.CanPlace ? validColor : invalidColor;

            if (previewSprite != null)
                previewSprite.color = color;

            if (rangeIndicator != null)
            {
                rangeIndicator.color = color;
                if (preview.TowerData?.attackData?.targeting != null)
                {
                    var range = GetAttackRange(preview.TowerData);
                    rangeIndicator.transform.localScale = Vector3.one * range * 2f;
                }
            }
        }

        private float GetAttackRange(Data.StaticData.Tower.TowerData data)
        {
            if (data.attackData?.targeting is Data.StaticData.Targeting.CircleTargetingData circle)
                return circle.radius;
            return 3f;
        }
    }
}
