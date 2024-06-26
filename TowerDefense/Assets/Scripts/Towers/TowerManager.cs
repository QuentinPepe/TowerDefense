using System;
using System.Collections.Generic;
using DG.Tweening;
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
        public event Action<Tower> OnTowerDestroy;

        public GridController gridController;
        [SerializeField] private AudioClip placeTowerSound;


        private void Awake()
        {
            _towers = new Dictionary<CellPosition, Tower>();

            gridController.OnHoverEnter += HandleHoverEnter;
            gridController.OnHoverLeave += HandleHoverLeave;
        }
        private void HandleHoverLeave(CellPosition obj)
        {
            Tower tower = GetTower(obj);
            if (tower != null)
            {
                tower.HideRadius();
            }
        }

        private void HandleHoverEnter(CellPosition obj)
        {
            Tower tower = GetTower(obj);
            if (tower != null)
            {
                tower.DrawRadius(tower.Data.range);
            }
        }

        public bool PlaceTower(TowerSO towerData, CellPosition position)
        {
            if (towerData.IsUnityNull() || !CanAfford(towerData.cost))
            {
                return false;
            }

            Vector3 worldPosition = new Vector3(position.X + 0.5f, 0, position.Z + 0.5f);

            Tower newTower = CreateTower(towerData, position, worldPosition);
            newTower.transform.localScale = Vector3.zero;
            newTower.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce);
            OnTowerPlaced?.Invoke(newTower);
            Game.Instance.Currency -= towerData.cost;
            return true;
        }
        private Tower CreateTower(TowerSO towerData, CellPosition position, Vector3 worldPosition)
        {

            GameObject towerPrefab = towerData.prefab;
            GameObject towerObject = Instantiate(towerPrefab, worldPosition, Quaternion.identity, transform);
            if (!towerObject.TryGetComponent(out Tower newTower)) throw new MissingComponentException("Tower component not found on tower prefab.");
            newTower.Initialize(towerData, position);
            _towers.Remove(position);
            _towers.Add(position, newTower);
            return newTower;
        }

        public Tower UpgradeTower(Tower tower)
        {
            if (tower == null || !tower.CanUpgrade() || !CanAfford(tower.Data.upgrade.cost))
            {
                Debug.LogError("Cannot upgrade tower: Invalid tower or insufficient funds.");
                return null;
            }

            Game.Instance.Currency -= tower.Data.upgrade.cost;
            TowerSO upgradeData = tower.Data.upgrade;
            Destroy(tower.gameObject);
            Tower upgradedTower = CreateTower(upgradeData, tower.CellPosition, tower.transform.position);
            OnTowerUpgraded?.Invoke(upgradedTower);

            return upgradedTower;
        }

        public void DestroyTower(Tower tower)
        {
            if (tower == null)
            {
                Debug.LogError("Cannot destroy tower: Invalid tower.");
                return;
            }

            _towers.Remove(tower.CellPosition);
            OnTowerDestroy?.Invoke(tower);
            Destroy(tower.gameObject);
        }

        public bool CanAfford(int cost)
        {
            return cost <= Game.Instance.Currency;
        }
        public bool IsCellOccupied(CellPosition position)
        {
            return _towers.ContainsKey(position);
        }

        public Tower GetTower(CellPosition position)
        {
            return _towers.TryGetValue(position, out Tower tower) ? tower : null;
        }
    }
}