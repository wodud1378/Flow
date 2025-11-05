using Flow.Sample.GamePlay.Configs;
using UnityEngine;

namespace Flow.Sample.DI.Configs
{
    public class Config : ScriptableObject, IConfig
    {
        [field: SerializeField] public int DetectBufferSize { get; private set; } = 32;
        [field: SerializeField] public int UpdateEntitySystemBufferSize { get; private set; } = 128;
        [field: SerializeField] public int EnemySpawnLimitPerFrame { get; private set; } = 4;
    }
}