using UnityEngine;
using System;

namespace Grid
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayer;

        private Camera _camera;
        private GridView _gridView;

        public event Action<int, int> OnCellClick;
        public event Action<int, int> OnHoverEnter;
        public event Action<int, int> OnHoverStay;
        public event Action<int, int> OnHoverLeave;

        private int? _lastHoveredX;
        private int? _lastHoveredY;

        private void Awake()
        {
            _camera = Camera.main;
            _gridView = GetComponent<GridView>();
            _gridView.CreateGrid(11, 11, 1f);
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

            if (Input.GetMouseButtonDown(0))
                OnCellClick?.Invoke(x, y);

            if (_lastHoveredX != x || _lastHoveredY != y)
            {
                if (_lastHoveredX.HasValue && _lastHoveredY.HasValue)
                    OnHoverLeave?.Invoke(_lastHoveredX.Value, _lastHoveredY.Value);

                OnHoverEnter?.Invoke(x, y);
                _lastHoveredX = x;
                _lastHoveredY = y;
            }
            else
            {
                OnHoverStay?.Invoke(x, y);
            }
        }
    }
}