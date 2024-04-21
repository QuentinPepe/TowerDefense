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
            if (_ghostTowerInstance.IsUnityNull()) return;
            Vector3 worldPosition = new Vector3(position.X, 0, position.Z);
            _ghostTowerInstance.transform.position = worldPosition;
            _ghostTowerInstance.SetActive(true);
            UpdateGhostGroundColor(position);
        }

        private void HideGhostTower(CellPosition position)
        {
            if (_ghostTowerInstance.IsUnityNull()) return;
            _ghostTowerInstance.SetActive(false);
        }

        private void UpdateGhostGroundColor(CellPosition position)
        {
            // TODO: Implement this method
        }

        private void SetGhostTowerAppearance()
        {
            if (_ghostTowerInstance.TryGetComponent(out Renderer renderer))
            {
                renderer.material.color = new Color(1, 1, 1, 0.5f); // Semi-transparent
            }
        }

        private void HandleCellPlacement(CellPosition position)
        {
            if (!CanPlaceTower(position)) return;
            PlaceTower(position);
            HideGhostTower(position);
        }

        private bool CanPlaceTower(CellPosition position)
        {
            return gridController.IsCellWalkable(position) && !towerManager.IsCellOccupied(position);
        }

        private void PlaceTower(CellPosition position)
        {
            towerManager.PlaceTower(_selectedTower, position);
        }
    }
}