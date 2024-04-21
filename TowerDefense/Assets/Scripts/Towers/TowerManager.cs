using System;
using System.Collections.Generic;
using Grid;
using Unity.VisualScripting;
using UnityEngine;

namespace Towers
{
    public class TowerManager : MonoBehaviour
    {
        private Dictionary<CellPosition, Tower> _towers;
        public event Action<Tower> OnTowerPlaced;
        public event Action<Tower> OnTowerUpgraded;
        public event Action<Tower> OnTowerSold;

        private void Awake()
        {
            _towers = new Dictionary<CellPosition, Tower>();
        }

        public bool PlaceTower(TowerSO towerData, CellPosition position)
        {
            if (towerData.IsUnityNull() || !CanAfford(towerData.cost))
            {
                return false;
            }

            Vector3 worldPosition = new Vector3(position.X, 0, position.Z);

            GameObject towerPrefab = towerData.prefab;
            GameObject towerObject = Instantiate(towerPrefab, worldPosition, Quaternion.identity, transform);
            if (!towerObject.TryGetComponent(out Tower newTower)) throw new MissingComponentException("Tower component not found on tower prefab.");
            newTower.Initialize(towerData, position);
            _towers.Add(position, newTower);
            OnTowerPlaced?.Invoke(newTower);
            return true;
        }

        public void UpgradeTower(Tower tower)
        {
            if (tower == null || !tower.CanUpgrade() || !CanAfford(tower.Data.cost))
            {
                Debug.LogError("Cannot upgrade tower: Invalid tower or insufficient funds.");
                return;
            }

            tower.Upgrade();
            OnTowerUpgraded?.Invoke(tower);
        }

        public void SellTower(Tower tower)
        {
            if (tower == null)
            {
                Debug.LogError("Cannot sell tower: Invalid tower.");
                return;
            }

            _towers.Remove(tower.CellPosition);
            OnTowerSold?.Invoke(tower);
            Destroy(tower.gameObject);
        }

        private bool CanAfford(int cost)
        {
            // TODO : player's currency check
            return true;
        }
        public bool IsCellOccupied(CellPosition position)
        {
            return _towers.ContainsKey(position);
        }
    }
}