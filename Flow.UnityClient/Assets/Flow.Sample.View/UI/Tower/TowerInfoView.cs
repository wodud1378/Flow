using System;
using Flow.Sample.Data.StaticData.Tower;
using Flow.Sample.GamePlay.Entities;
using Flow.Sample.GamePlay.Input;
using Flow.Sample.GamePlay.Services;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Flow.Sample.View.UI.Tower
{
    public class TowerInfoView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI towerNameText;
        [SerializeField] private TextMeshProUGUI statsText;
        [SerializeField] private Button sellButton;
        [SerializeField] private TextMeshProUGUI sellValueText;

        private TowerBuildService _buildService;
        private IDisposable _subscription;

        private TowerEntity _selectedTower;
        private TowerData _towerData;

        [Inject]
        public void Initialize(InputEvents inputEvents, TowerBuildService buildService)
        {
            _buildService = buildService;
            _subscription = inputEvents.OnTowerClicked.Subscribe(OnTowerClicked);

            sellButton?.onClick.AddListener(OnSellClicked);
            Hide();
        }

        private void OnDestroy()
        {
            _subscription?.Dispose();
        }

        private void OnTowerClicked(TowerEntity tower)
        {
            _selectedTower = tower;
            Show(tower);
        }

        private void Show(TowerEntity tower)
        {
            panel?.SetActive(true);

            if (towerNameText != null)
                towerNameText.text = tower.name;

            var status = tower.GetSystemComponent<GamePlay.Components.StatusComponent>();
            if (statsText != null && status != null)
            {
                statsText.text = $"Damage: {status.Damage}\nCrit: {status.CriticalRate:P0}";
            }

            // Get sell value from TowerData if available
            var sellValue = 50; // Default
            if (sellValueText != null)
                sellValueText.text = $"Sell: {sellValue}";
        }

        private void Hide()
        {
            panel?.SetActive(false);
            _selectedTower = null;
        }

        private void OnSellClicked()
        {
            if (_selectedTower == null || !_selectedTower.IsValid)
                return;

            var sellValue = 50; // Get from TowerData
            _buildService.SellTower(_selectedTower, sellValue);
            Hide();
        }

        private void Update()
        {
            // Close on right click or escape
            if (panel.activeSelf && (UnityEngine.Input.GetMouseButtonDown(1) || UnityEngine.Input.GetKeyDown(KeyCode.Escape)))
            {
                Hide();
            }
        }
    }
}
