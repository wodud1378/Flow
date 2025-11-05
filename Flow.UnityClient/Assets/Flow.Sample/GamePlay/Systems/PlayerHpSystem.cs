using System.Collections.Generic;
using Flow.Sample.GamePlay.Events;
using Flow.Sample.GamePlay.Systems.Base;
using Flow.Sample.GamePlay.Systems.Interfaces;
using R3;
using UnityEngine;
using VContainer;

namespace Flow.Sample.GamePlay.Systems
{
    public class PlayerHpSystem : BaseUpdateSystem
    {
        private readonly IPlayerStatusProvider _statusProvider;
        private readonly PlayerEvents _events;
        private readonly Queue<float> _addValueQueue = new();
            
        private float _remainHp;
        private float _hpRecoveryElapsed;
        
        [Inject]
        public PlayerHpSystem(IPlayerStatusProvider statusProvider, PlayerEvents events)
        {
            _statusProvider = statusProvider;
            _events = events;
        }

        protected override void OnStartRunning()
        {
            _remainHp = _statusProvider.Hp;
        }

        protected override void OnUpdate(float deltaTime)
        {
            _hpRecoveryElapsed += deltaTime;
            if (_hpRecoveryElapsed >= _statusProvider.HpRecoveryTime)
            {
                _addValueQueue.Enqueue(_statusProvider.Hp * _statusProvider.HpRecoveryRate);
            }
            
            while (_addValueQueue.Count > 0)
            {
                var value = _addValueQueue.Dequeue();
                var prev = _remainHp;
                _remainHp = Mathf.Clamp(prev + value, 0f, _statusProvider.Hp);
                _events.HpChangedStream.OnNext(_remainHp);
            }

            if (_remainHp <= 0f)
            {
                _events.PlayerDeadStream.OnNext(Unit.Default);
                Enabled = false;
            }
        }

        public void RequestIncrease(float value) => _addValueQueue.Enqueue(value);

        public void RequestDecrease(float value) => _addValueQueue.Enqueue(-value);
    }
}