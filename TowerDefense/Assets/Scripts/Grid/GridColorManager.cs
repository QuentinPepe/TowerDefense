using UnityEngine;
using System;

namespace Grid
{
    public class GridColorManager : MonoBehaviour
    {
        private GridController _gridController;
        private GridView _gridView;
        private readonly Color _defaultColor = Color.white;
        private readonly Color _hoverColor = Color.red;
        private readonly Color _clickColor = Color.blue;

        private void Awake()
        {
            _gridController = GetComponent<GridController>();
            _gridView = GetComponent<GridView>();
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

        private void HandleCellClick(int x, int y)
        {
            SetCellColor(x, y, _clickColor);
        }

        private void HandleHoverEnter(int x, int y)
        {
            SetCellColor(x, y, _hoverColor);
        }

        private void HandleHoverLeave(int x, int y)
        {
            SetCellColor(x, y, _defaultColor);
        }

        private void SetCellColor(int x, int y, Color color)
        {
            GameObject cell = _gridView.GetCell(x, y);
            if (cell == null) return;
            Renderer cellRenderer = cell.GetComponent<Renderer>();
            if (cellRenderer != null)
            {
                cellRenderer.material.color = color;
            }
        }
    }
}