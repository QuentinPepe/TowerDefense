using System;
using UnityEngine;

namespace Grid
{
    public class GridState : MonoBehaviour
    {
        public int Width { get; private set; } = 11;
        public int Height { get; private set; } = 11;

        [SerializeField] private int[] stateMatrix;
        [SerializeField] private GridView gridView;

        public void InitializeMatrix(int width, int height)
        {
            if (width == Width && height == Height) return;
            Width = width;
            Height = height;
            stateMatrix = new int[width * height];
            ResetMatrix();
        }

        public void ResetMatrix()
        {
            for (int i = 0; i < stateMatrix.Length; i++)
            {
                stateMatrix[i] = 0;
            }
            gridView.UpdateGrid();
        }

        public int GetState(CellPosition cellPosition)
        {
            int x = cellPosition.X;
            int z = cellPosition.Z;
            if (x < 0 || x >= Width || z < 0 || z >= Height)
                throw new ArgumentOutOfRangeException("x and y must be within the grid boundaries.");
            return stateMatrix[z * Width + x];
        }

        private void SetState(CellPosition cellPosition, int state)
        {
            int x = cellPosition.X;
            int z = cellPosition.Z;
            if (x < 0 || x >= Width || z < 0 || z >= Height)
                throw new ArgumentOutOfRangeException("x and y must be within the grid boundaries.");
            stateMatrix[z * Width + x] = state;
            gridView.UpdateGrid();
        }

        public void SetWalkable(CellPosition cellPosition, bool walkable)
        {
            SetState(cellPosition, walkable ? 1 : 0);
        }
    }
}