using System;
using Flow.Sample.GamePlay.Events;
using Flow.Sample.GamePlay.Events.Models;
using Flow.Sample.GamePlay.Models;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Flow.Sample.View.UI.HUD
{
    public class GameHUDView : MonoBehaviour
    {
        [Header("Player Info")]
        [SerializeField] private Image hpFillImage;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI scoreText;

        [Header("Wave Info")]
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private TextMeshProUGUI enemyCountText;

        [Header("Game State")]
        [SerializeField] private TextMeshProUGUI stateText;
        [SerializeField] private GameObject startPrompt;

        private IDisposable _subscriptions;

        [Inject]
        public void Initialize(
            PlayerEvents playerEvents)
        {
            _subscriptions = Disposable.Combine(
                playerEvents.OnHpChanged.Subscribe(OnHpChanged),
                playerEvents.OnMeticsUpdated.Subscribe(OnMetricsUpdated),
                playerEvents.OnWaveUpdated.Subscribe(OnWaveUpdated)
            );
        }

        private void OnDestroy()
        {
            _subscriptions?.Dispose();
        }

        private void OnHpChanged(HpChanged hpChanged)
        {
            var ratio = hpChanged.Current / hpChanged.Max;

            hpFillImage.fillAmount = ratio;
            hpText.text = $"{(int)hpChanged.Current} / {(int)hpChanged.Max}";
        }

        private void OnMetricsUpdated(Metrics metrics)
        {
            goldText.text = $"{metrics.Gold}";
            scoreText.text = $"{metrics.Score}";
        }

        private void OnWaveUpdated(Wave wave)
        {
            waveText.text = $"Wave {wave.Number}";
            enemyCountText.text = $"Killed: {wave.EnemyKilled}";
        }
    }
}