using System;
using Flow.Sample.Data.StaticData.Tower;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Flow.Sample.View.UI.Tower
{
    public class TowerButtonView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI costText;
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