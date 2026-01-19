using System;
using System.Collections.Generic;
using Flow.Sample.Data.StaticData.Tower;
using Flow.Sample.GamePlay.Input;
using Flow.Sample.GamePlay.Systems;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Flow.Sample.View.UI.Tower
{
    public class TowerSelectionView : MonoBehaviour
    {
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private TowerButtonView buttonPrefab;
        [SerializeField] private List<TowerData> availableTowers;

        private InputEvents _inputEvents;
        private ResourceSystem _resourceSystem;
        private readonly List<TowerButtonView> _buttons = new();

        [Inject]
        public void Initialize(InputEvents inputEvents, ResourceSystem resourceSystem)
        {
            _inputEvents = inputEvents;
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

        private void OnTowerButtonClicked(TowerData data)
        {
            _inputEvents.TowerSelectedStream.OnNext(data);
        }
    }

    public class TowerButtonView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image iconImage;
        [SerializeField] private TMPro.TextMeshProUGUI costText;
        [SerializeField] private CanvasGroup canvasGroup;

        public TowerData TowerData { get; private set; }

        private Action<TowerData> _onClick;

        public void Setup(TowerData data, Action<TowerData> onClick)
        {
            TowerData = data;
            _onClick = onClick;

            if (iconImage != null && data.icon != null)
                iconImage.sprite = data.icon;

            if (costText != null)
                costText.text = $"{data.buildCost}";

            button?.onClick.AddListener(OnClick);
        }

        public void SetInteractable(bool interactable)
        {
            if (button != null)
                button.interactable = interactable;

            if (canvasGroup != null)
                canvasGroup.alpha = interactable ? 1f : 0.5f;
        }

        private void OnClick()
        {
            _onClick?.Invoke(TowerData);
        }
    }
}
