using UnityEngine;

namespace Flow.Sample.GamePlay.Systems.Interfaces
{
    public interface ITowerInstallValidator
    {
        bool CanInstall(Vector2 position, float radius);
        bool HasEnoughGold(int cost);
    }
}
