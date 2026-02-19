using System;
using Flow.Sample.GamePlay.States;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace Flow.Sample.View.UI.GameState
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button quitButton;

        private IDisposable _subscription;

        [Inject]
        public void Initialize(GameStateEvents events)
        {
            _subscription = events.OnGameEnd.Subscribe(OnGameEnd);

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
            if (victory)
                return;

            Show();
        }

        private void Show()
        {
            panel?.SetActive(true);
            Time.timeScale = 0f;

            titleText.text = "GAME OVER";
            messageText.text = "Your base has been destroyed!";
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