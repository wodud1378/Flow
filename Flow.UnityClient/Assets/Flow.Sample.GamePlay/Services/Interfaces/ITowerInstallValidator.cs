using Flow.Sample.GamePlay.Entities;
using UnityEngine;

namespace Flow.Sample.GamePlay.Services.Interfaces
{
    public interface ITowerInstallValidator
    {
        public bool IsValid(TowerEntity tower, Vector2 position);
    }
}