using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Grid
{
    public class GridState : MonoBehaviour
    {
        [SerializeField] private int width = 11;
        [SerializeField] private int height = 11;
        [SerializeField] private int[] stateMatrix;
        [SerializeField] private GridView gridView;

        public int Width => width;
        public int Height => height;

        public void InitializeMatrix(int width, int height)
        {
            if (width == this.width && height == this.height) return;
            Debug.Log($"Initializing grid with dimensions {width}x{height}");
            this.width = width;
            this.height = height;
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
            if (x < 0 || x >= width || z < 0 || z >= height)
                throw new ArgumentOutOfRangeException("x and y must be within the grid boundaries.");
            return stateMatrix[z * width + x];
        }

        private void SetState(CellPosition cellPosition, int state)
        {
            int x = cellPosition.X;
            int z = cellPosition.Z;
            if (x < 0 || x >= width || z < 0 || z >= height)
                throw new ArgumentOutOfRangeException("x and y must be within the grid boundaries.");
            stateMatrix[z * width + x] = state;
            gridView.UpdateGrid();
        }

        public void SetWalkable(CellPosition cellPosition, bool walkable)
        {
            SetState(cellPosition, walkable ? 1 : 0);
        }
    }
}