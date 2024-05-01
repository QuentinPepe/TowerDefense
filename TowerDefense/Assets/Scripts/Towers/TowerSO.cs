using UnityEngine;

namespace Towers
{
    [CreateAssetMenu(fileName = "New Tower", menuName = "Tower")]
    public class TowerSO : ScriptableObject
    {
        public string towerName;
        public GameObject prefab;
        public int level;
        public int cost;
        public int damage;
        public float fireRate;
        public float range;
        public TowerSO upgrade; // Reference to an upgraded version of this tower
        public Sprite icon;
    }
}