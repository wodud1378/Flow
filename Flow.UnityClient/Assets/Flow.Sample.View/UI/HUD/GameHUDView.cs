using System;
using Flow.Sample.GamePlay.Events;
using Flow.Sample.GamePlay.Models;
using Flow.Sample.GamePlay.States;
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

        private float _maxHp = 100f;
        private IDisposable _subscriptions;

        [Inject]
        public void Initialize(
            PlayerEvents playerEvents,
            GameStateEvents stateEvents,
            GamePlay.Systems.Interfaces.IPlayerStatusProvider statusProvider)
        {
            _maxHp = statusProvider.Hp;

            _subscriptions = Disposable.Combine(
                playerEvents.OnHpChanged.Subscribe(OnHpChanged),
                playerEvents.OnMeticsUpdated.Subscribe(OnMetricsUpdated),
                playerEvents.OnWaveUpdated.Subscribe(OnWaveUpdated),
                stateEvents.OnStateChanged.Subscribe(OnStateChanged)
            );
        }

        private void OnDestroy()
        {
            _subscriptions?.Dispose();
        }

        private void OnHpChanged(float hp)
        {
            var ratio = hp / _maxHp;

            if (hpFillImage != null)
                hpFillImage.fillAmount = ratio;

            if (hpText != null)
                hpText.text = $"{(int)hp} / {(int)_maxHp}";
        }

        private void OnMetricsUpdated(Metrics metrics)
        {
            if (goldText != null)
                goldText.text = $"{metrics.Gold}";

            if (scoreText != null)
                scoreText.text = $"{metrics.Score}";
        }

        private void OnWaveUpdated(Wave wave)
        {
            if (waveText != null)
                waveText.text = $"Wave {wave.Number}";

            if (enemyCountText != null)
                enemyCountText.text = $"Killed: {wave.EnemyKilled}";
        }

        private void OnStateChanged(GameState state)
        {
            if (stateText != null)
            {
                stateText.text = state switch
                {
                    GameState. => "Press SPACE to Start",
                    GameState.Playing => "",
                    GameState.Paused => "PAUSED",
                    GameState.GameOver => "GAME OVER",
                    GameState.Victory => "VICTORY!",
                    _ => ""
                };
            }

            if (startPrompt != null)
                startPrompt.SetActive(state == GameState.Ready);
        }
    }
}
