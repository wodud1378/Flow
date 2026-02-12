using Flow.Sample.GamePlay.Systems.Interfaces;
using UnityEngine;

namespace Flow.Sample.GamePlay.Providers
{
    public class PlayerStatusProvider : MonoBehaviour, IPlayerStatusProvider
    {
        [SerializeField] private float hp = 100f;
        [SerializeField] private float hpRecoveryRate = 0.01f;
        [SerializeField] private float hpRecoveryTime = 10f;

        public float Hp => hp;
        public float HpRecoveryRate => hpRecoveryRate;
        public float HpRecoveryTime => hpRecoveryTime;
    }
}
