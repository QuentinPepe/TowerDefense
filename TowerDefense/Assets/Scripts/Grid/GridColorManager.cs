using Unity.VisualScripting;
using UnityEngine;

namespace Grid
{
    public class GridColorManager : MonoBehaviour
    {
        private GridController _gridController;
        private readonly Color _defaultColor = Color.white;
        private readonly Color _hoverColor = Color.red;
        private readonly Color _clickColor = Color.blue;

        private void Awake()
        {
            _gridController = GetComponent<GridController>();
        }

        private void OnEnable()
        {
            _gridController.OnCellClick += HandleCellClick;
            _gridController.OnHoverEnter += HandleHoverEnter;
            _gridController.OnHoverLeave += HandleHoverLeave;
        }

        private void OnDisable()
        {
            _gridController.OnCellClick -= HandleCellClick;
            _gridController.OnHoverEnter -= HandleHoverEnter;
            _gridController.OnHoverLeave -= HandleHoverLeave;
        }

        private void HandleCellClick(CellPosition position)
        {
            if (_gridController.IsCellWalkable(position)) return;
            SetCellColor(position, _clickColor);
        }

        private void HandleHoverEnter(CellPosition position)
        {
            if (_gridController.IsCellWalkable(position)) return;
            SetCellColor(position, _hoverColor);
        }

        private void HandleHoverLeave(CellPosition position)
        {
            if (_gridController.IsCellWalkable(position)) return;
            SetCellColor(position, _defaultColor);
        }

        private void SetCellColor(CellPosition position, Color color)
        {
            GameObject cell = _gridController.GetCell(position);
            if (cell.IsUnityNull()) return;
            if (cell.TryGetComponent(out Renderer cellRenderer))
            {
                cellRenderer.material.color = color;
            }
        }
    }
}