using System;
using Flow.Sample.GamePlay.States;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace Flow.Sample.View.UI.GameState
{
    public class PauseView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button quitButton;

        private GameStateSystem _gameStateSystem;
        private IDisposable _subscription;

        [Inject]
        public void Initialize(GameStateEvents events, GameStateSystem gameStateSystem)
        {
            _gameStateSystem = gameStateSystem;
            _subscription = events.OnStateChanged.Subscribe(OnStateChanged);

            resumeButton?.onClick.AddListener(OnResumeClicked);
            restartButton?.onClick.AddListener(OnRestartClicked);
            quitButton?.onClick.AddListener(OnQuitClicked);

            panel?.SetActive(false);
        }

        private void OnDestroy()
        {
            _subscription?.Dispose();
        }

        private void OnStateChanged(GameState state)
        {
            panel?.SetActive(state == GameState.Paused);
        }

        private void OnResumeClicked()
        {
            _gameStateSystem.ResumeGame();
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
