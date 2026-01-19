using System;
using Flow.Sample.GamePlay.Events;
using Flow.Sample.GamePlay.Models;
using R3;
using TMPro;
using UnityEngine;
using VContainer;

namespace Flow.Sample.View.UI.Components
{
    public class WaveView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI waveNumberText;
        [SerializeField] private TextMeshProUGUI enemyKilledText;

        private IDisposable _subscription;

        [Inject]
        public void Initialize(PlayerEvents events)
        {
            _subscription = events.OnWaveUpdated.Subscribe(OnWaveUpdated);
        }

        private void OnDestroy()
        {
            _subscription?.Dispose();
        }

        private void OnWaveUpdated(Wave wave)
        {
            if (waveNumberText != null)
                waveNumberText.text = $"Wave {wave.Number}";

            if (enemyKilledText != null)
                enemyKilledText.text = $"Killed: {wave.EnemyKilled}";
        }
    }
}
