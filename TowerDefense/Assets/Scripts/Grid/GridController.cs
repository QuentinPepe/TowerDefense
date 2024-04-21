using UnityEngine;
using System;

namespace Grid
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayer;

        private Camera _camera;
        private GridView _gridView;
        private GridState _gridState;

        public event Action<CellPosition> OnCellClick;
        public event Action<CellPosition> OnHoverEnter;
        public event Action<CellPosition> OnHoverLeave;

        private CellPosition? _lastHoveredPosition;

        private void Awake()
        {
            _camera = Camera.main;
            _gridView = GetComponent<GridView>();
            _gridState = GetComponent<GridState>();
        }

        private void Update()
        {
            HandleMouse();
        }

        private void HandleMouse()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer)) return;
            _gridView.GridModel.GetXY(hit.point, out int x, out int y);
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
            return _gridState.GetState(cellPosition) == 1;
        }

        public GameObject GetCell(CellPosition position)
        {
            return _gridView.GetCell(position);
        }
    }
}