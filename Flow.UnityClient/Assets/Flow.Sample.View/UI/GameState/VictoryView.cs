using System;
using Flow.Sample.GamePlay.Events;
using Flow.Sample.GamePlay.Models;
using Flow.Sample.GamePlay.States;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace Flow.Sample.View.UI.GameState
{
    public class VictoryView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI statsText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button quitButton;

        private IDisposable _subscription;
        private Metrics _lastMetrics;

        [Inject]
        public void Initialize(GameStateEvents stateEvents, PlayerEvents playerEvents)
        {
            _subscription = Disposable.Combine(
                stateEvents.OnGameEnd.Subscribe(OnGameEnd),
                playerEvents.OnMeticsUpdated.Subscribe(m => _lastMetrics = m)
            );

            restartButton?.onClick.AddListener(OnRestartClicked);
            quitButton?.onClick.AddListener(OnQuitClicked);

            panel?.SetActive(false);
        }

        private void OnDestroy()
        {
            _subscription?.Dispose();
        }

        private void OnGameEnd(bool victory)
        {
            if (!victory)
                return; // GameOverView handles this

            Show();
        }

        private void Show()
        {
            panel?.SetActive(true);

            if (titleText != null)
                titleText.text = "VICTORY!";

            if (scoreText != null)
                scoreText.text = $"Score: {_lastMetrics.Score}";

            if (statsText != null)
                statsText.text = $"Gold Remaining: {_lastMetrics.Gold}";
        }

        private void OnRestartClicked()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnQuitClicked()
        {
            Time.timeScale = 1f;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
