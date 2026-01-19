using Flow.Sample.Data.StaticData.Attack;
using UnityEngine;

namespace Flow.Sample.Data.StaticData.Tower
{
    public abstract class TowerData : ScriptableObject
    {
        [Header("Tower Info")]
        public string towerName;
        public Sprite icon;
        public GameObject prefab;

        [Header("Cost")]
        public int buildCost = 100;
        public int sellValue = 50;

        [Header("Size")]
        public float radius = 0.5f;

        [Header("Attack")]
        public AttackData attackData;
    }
}
