using System;
using System.Collections.Generic;
using Flow.Sample.GamePlay.Entities;
using Flow.Sample.GamePlay.Events;
using Flow.Sample.GamePlay.Logics.Models;
using Flow.Sample.GamePlay.Models;
using Flow.Sample.GamePlay.Systems;
using R3;
using TMPro;
using UnityEngine;
using VContainer;

namespace Flow.Sample.View.Contents.Entity
{
    public class DamagePopup : MonoBehaviour
    {
        [SerializeField] private TextMeshPro textPrefab;
        [SerializeField] private float floatSpeed = 1f;
        [SerializeField] private float fadeDuration = 0.8f;
        [SerializeField] private Color normalDamageColor = Color.white;
        [SerializeField] private Color criticalDamageColor = Color.yellow;
        [SerializeField] private float criticalScale = 1.5f;

        private PoolSystem _poolSystem;
        private IDisposable _subscription;
        private readonly List<PopupInstance> _activePopups = new();

        private struct PopupInstance
        {
            public TextMeshPro Text;
            public float ElapsedTime;
            public Vector3 StartPosition;
        }

        [Inject]
        public void Initialize(CombatEvents combatEvents, PoolSystem poolSystem)
        {
            _poolSystem = poolSystem;
            _subscription = combatEvents.OnDamaged.Subscribe(OnDamaged);
        }

        private void OnDestroy()
        {
            _subscription?.Dispose();
        }

        private void Update()
        {
            UpdatePopups(Time.deltaTime);
        }

        private void OnDamaged(Damaged damaged)
        {
            if (!IsValidEntity(damaged.Attacker) ||
                !IsValidEntity(damaged.Victim))
                return;
            
            var position = damaged.Victim.transform.position + Vector3.up * 0.5f;
            var damage = damaged.Damage;
            SpawnPopup(position, damage.Value, damage is CriticalDamage);
        }

        private bool IsValidEntity(BaseEntity entity) => entity != null && entity.IsValid;

        private void SpawnPopup(Vector3 position, float damage, bool isCritical)
        {
            var text = _poolSystem.GetObject(textPrefab);
            text.transform.position = position;
            text.text = ((int)damage).ToString();
            text.color = isCritical ? criticalDamageColor : normalDamageColor;
            text.transform.localScale = Vector3.one * (isCritical ? criticalScale : 1f);

            _activePopups.Add(new PopupInstance
            {
                Text = text,
                ElapsedTime = 0f,
                StartPosition = position
            });
        }

        private void UpdatePopups(float deltaTime)
        {
            for (int i = _activePopups.Count - 1; i >= 0; i--)
            {
                var popup = _activePopups[i];
                popup.ElapsedTime += deltaTime;

                var progress = popup.ElapsedTime / fadeDuration;

                // Move up
                popup.Text.transform.position = popup.StartPosition + Vector3.up * (floatSpeed * popup.ElapsedTime);

                // Fade out
                var color = popup.Text.color;
                color.a = 1f - progress;
                popup.Text.color = color;

                _activePopups[i] = popup;

                if (progress >= 1f)
                {
                    popup.Text.gameObject.SetActive(false);
                    _activePopups.RemoveAt(i);
                }
            }
        }
    }
}
