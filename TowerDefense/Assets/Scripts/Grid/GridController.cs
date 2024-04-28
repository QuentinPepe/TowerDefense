using UnityEngine;
using System;
using UnityEngine.Serialization;

namespace Grid
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private GridView gridView;
        [SerializeField] private GridState gridState;

        private Camera _camera;

        public event Action<CellPosition> OnCellClick;
        public event Action<CellPosition> OnHoverEnter;
        public event Action<CellPosition> OnHoverLeave;

        private CellPosition? _lastHoveredPosition;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            HandleMouse();
        }

        private void HandleMouse()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer)) return;
            gridView.GridModel.GetXY(hit.point, out int x, out int y);
            CellPosition position = new CellPosition(x, y);

            if (Input.GetMouseButtonDown(0))
                OnCellClick?.Invoke(position);

            if (_lastHoveredPosition.HasValue && _lastHoveredPosition.Value == position) return;
            if (_lastHoveredPosition.HasValue)
                OnHoverLeave?.Invoke(_lastHoveredPosition.Value);

            OnHoverEnter?.Invoke(position);
            _lastHoveredPosition = position;
        }

        public bool IsCellWalkable(CellPosition cellPosition)
        {
            return gridState.GetState(cellPosition) >= 1;
        }

        public GameObject GetCell(CellPosition position)
        {
            return gridView.GetCell(position);
        }
    }
}