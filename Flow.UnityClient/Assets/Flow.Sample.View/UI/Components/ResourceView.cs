using System;
using Flow.Sample.GamePlay.Events;
using Flow.Sample.GamePlay.Models;
using R3;
using TMPro;
using UnityEngine;
using VContainer;

namespace Flow.Sample.View.UI.Components
{
    public class ResourceView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI scoreText;

        private IDisposable _subscription;

        [Inject]
        public void Initialize(PlayerEvents events)
        {
            _subscription = events.OnMeticsUpdated.Subscribe(OnMetricsUpdated);
        }

        private void OnDestroy()
        {
            _subscription?.Dispose();
        }

        private void OnMetricsUpdated(Metrics metrics)
        {
            if (goldText != null)
                goldText.text = $"{metrics.Gold}";

            if (scoreText != null)
                scoreText.text = $"{metrics.Score}";
        }
    }
}
