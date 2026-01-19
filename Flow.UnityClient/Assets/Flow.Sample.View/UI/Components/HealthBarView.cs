using Flow.Sample.GamePlay.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Flow.Sample.View.UI.Components
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private Gradient colorGradient;
        [SerializeField] private bool followTarget = true;
        [SerializeField] private Vector3 offset = new Vector3(0, 1f, 0);

        private StatusComponent _target;
        private Transform _targetTransform;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void SetTarget(StatusComponent target)
        {
            _target = target;
            _targetTransform = target?.transform;
            UpdateHealth();
        }

        private void LateUpdate()
        {
            if (_target == null || !_target.Owner.IsValid)
            {
                gameObject.SetActive(false);
                return;
            }

            UpdateHealth();
            UpdatePosition();
        }

        private void UpdateHealth()
        {
            if (_target == null)
                return;

            var ratio = _target.RemainHp / _target.MaxHp;
            ratio = Mathf.Clamp01(ratio);

            if (fillImage != null)
            {
                fillImage.fillAmount = ratio;
                if (colorGradient != null)
                    fillImage.color = colorGradient.Evaluate(ratio);
            }
        }

        private void UpdatePosition()
        {
            if (!followTarget || _targetTransform == null)
                return;

            transform.position = _targetTransform.position + offset;
        }
    }
}
