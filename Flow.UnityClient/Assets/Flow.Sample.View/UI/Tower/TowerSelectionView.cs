using System.Collections.Generic;
using Flow.Sample.Data.StaticData.Tower;
using Flow.Sample.GamePlay.Input;
using Flow.Sample.GamePlay.Services.Interfaces;
using Flow.Sample.GamePlay.Systems;
using R3;
using UnityEngine;
using VContainer;

namespace Flow.Sample.View.UI.Tower
{
    public class TowerSelectionView : MonoBehaviour, ITowerSelector
    {
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private TowerButtonView buttonPrefab;
        [SerializeField] private List<TowerData> availableTowers;

        public ReadOnlyReactiveProperty<TowerData> Selected => _selected;
        
        private readonly List<TowerButtonView> _buttons = new();
        private readonly ReactiveProperty<TowerData> _selected = new();
        
        private ResourceSystem _resourceSystem;

        [Inject]
        public void Initialize(ResourceSystem resourceSystem)
        {
            _resourceSystem = resourceSystem;

            CreateButtons();
        }

        private void CreateButtons()
        {
            foreach (var towerData in availableTowers)
            {
                var button = Instantiate(buttonPrefab, buttonContainer);
                button.Setup(towerData, OnTowerButtonClicked);
                _buttons.Add(button);
            }
        }

        private void Update()
        {
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            foreach (var button in _buttons)
            {
                var canAfford = _resourceSystem.CanAfford(button.TowerData.buildCost);
                button.SetInteractable(canAfford);
            }
        }

        private void OnTowerButtonClicked(TowerData data) => _selected.Value = data;
    }
}
