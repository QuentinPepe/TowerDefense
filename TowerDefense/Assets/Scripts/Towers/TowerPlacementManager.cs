using Grid;
using Unity.VisualScripting;
using UnityEngine;

namespace Towers
{
    public class TowerPlacementManager : MonoBehaviour
    {
        [SerializeField] private TowerManager towerManager;
        [SerializeField] private TowerSelectionUI towerSelectionUI;
        [SerializeField] private GridController gridController;

        private TowerSO _selectedTower;
        private GameObject _ghostTowerInstance;

        private void OnEnable()
        {
            towerSelectionUI.OnTowerSelected += HandleTowerSelected;
            gridController.OnCellClick += HandleCellPlacement;
            gridController.OnHoverEnter += ShowGhostTower;
            gridController.OnHoverLeave += HideGhostTower;
        }

        private void OnDisable()
        {
            towerSelectionUI.OnTowerSelected -= HandleTowerSelected;
            gridController.OnCellClick -= HandleCellPlacement;
            gridController.OnHoverEnter -= ShowGhostTower;
            gridController.OnHoverLeave -= HideGhostTower;
        }

        private void HandleTowerSelected(TowerSO tower)
        {
            _selectedTower = tower;

            if (_ghostTowerInstance != null) Destroy(_ghostTowerInstance);

            if (_selectedTower == null || _selectedTower.prefab == null) return;

            _ghostTowerInstance = Instantiate(_selectedTower.prefab);
            SetGhostTowerAppearance();
        }

        private void ShowGhostTower(CellPosition position)
        {
            if (_selectedTower.IsUnityNull()) return;
            if (_ghostTowerInstance.IsUnityNull()) return;
            Vector3 worldPosition = new Vector3(position.X, 0, position.Z);
            _ghostTowerInstance.transform.position = worldPosition;
            _ghostTowerInstance.SetActive(true);
            UpdateGroundColor(position);
        }

        private void HideGhostTower(CellPosition position)
        {
            if (_ghostTowerInstance.IsUnityNull()) return;
            _ghostTowerInstance.SetActive(false);
        }

        private void UpdateGroundColor(CellPosition position)
        {
            // TODO: Implement this method
        }

        private void SetGhostTowerAppearance()
        {
            // TODO: Make the ghost tower transparent
        }

        private void HandleCellPlacement(CellPosition position)
        {
            if (!CanPlaceTower(position)) return;
            if (!PlaceTower(position)) return;
            Destroy(_ghostTowerInstance);
            _selectedTower = null;
        }

        private bool CanPlaceTower(CellPosition position)
        {
            return !gridController.IsCellWalkable(position) && !towerManager.IsCellOccupied(position);
        }

        private bool PlaceTower(CellPosition position)
        {
            return towerManager.PlaceTower(_selectedTower, position);
        }
    }
}