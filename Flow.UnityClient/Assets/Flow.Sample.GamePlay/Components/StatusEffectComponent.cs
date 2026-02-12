using System.Collections.Generic;
using Flow.Sample.GamePlay.Components.Interfaces;
using Flow.Sample.GamePlay.Entities;
using UnityEngine;

namespace Flow.Sample.GamePlay.Components
{
    public class StatusEffectComponent : MonoBehaviour, IComponent
    {
        [field: SerializeField] public BaseEntity Owner { get; private set; }

        private readonly List<StatusEffect> _effects = new();
        private readonly List<StatusEffect> _toRemove = new();

        public float SpeedMultiplier { get; private set; } = 1f;
        public float DamageMultiplier { get; private set; } = 1f;

        private void OnValidate()
        {
            if (Owner == null)
                Owner = GetComponent<BaseEntity>();
        }

        public void AddEffect(StatusEffect effect)
        {
            // Check if same type effect exists and refresh duration
            for (int i = 0; i < _effects.Count; i++)
            {
                if (_effects[i].Type == effect.Type)
                {
                    _effects[i] = effect;
                    RecalculateModifiers();
                    return;
                }
            }

            _effects.Add(effect);
            RecalculateModifiers();
        }

        public void RemoveEffect(StatusEffectType type)
        {
            _effects.RemoveAll(e => e.Type == type);
            RecalculateModifiers();
        }

        public void UpdateEffects(float deltaTime)
        {
            _toRemove.Clear();

            for (int i = 0; i < _effects.Count; i++)
            {
                var effect = _effects[i];
                effect.RemainingDuration -= deltaTime;
                _effects[i] = effect;

                if (effect.RemainingDuration <= 0)
                    _toRemove.Add(effect);
            }

            foreach (var effect in _toRemove)
                _effects.Remove(effect);

            if (_toRemove.Count > 0)
                RecalculateModifiers();
        }

        public void ClearAllEffects()
        {
            _effects.Clear();
            SpeedMultiplier = 1f;
            DamageMultiplier = 1f;
        }

        private void RecalculateModifiers()
        {
            SpeedMultiplier = 1f;
            DamageMultiplier = 1f;

            foreach (var effect in _effects)
            {
                switch (effect.Type)
                {
                    case StatusEffectType.Slow:
                        SpeedMultiplier *= effect.Value;
                        break;
                    case StatusEffectType.Weaken:
                        DamageMultiplier *= effect.Value;
                        break;
                }
            }
        }
    }

    public struct StatusEffect
    {
        public StatusEffectType Type;
        public float Value;
        public float RemainingDuration;

        public StatusEffect(StatusEffectType type, float value, float duration)
        {
            Type = type;
            Value = value;
            RemainingDuration = duration;
        }
    }

    public enum StatusEffectType
    {
        None,
        Slow,
        Weaken,
        Poison,
        Stun
    }
}
