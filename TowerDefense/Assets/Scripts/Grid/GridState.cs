using System;
using System.Linq;
using CreatureS;
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
        [SerializeField] private CreatureManager creatureManager;

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
            int x = cellPosition.X - 15;
            int z = cellPosition.Z - 15;
            if (x < 0 || x >= width || z < 0 || z >= height)
                throw new IndexOutOfRangeException("Cell position out of bounds");
            return stateMatrix[z * width + x];
        }

        private void SetState(CellPosition cellPosition, int state)
        {
            int x = cellPosition.X - GridView.ExtensionLayers;
            int z = cellPosition.Z - GridView.ExtensionLayers;
            if (x < 0 || x >= width || z < 0 || z >= height)
                throw new IndexOutOfRangeException("Cell position out of bounds");
            stateMatrix[z * width + x] = state;
            gridView.UpdateGrid();

            if (state == 2)
            {
                creatureManager.SetStartPosition(new CellPosition(x + GridView.ExtensionLayers, z + GridView.ExtensionLayers));
            }
            else if (state == 3)
            {
                creatureManager.SetEndPosition(new CellPosition(x + GridView.ExtensionLayers, z + GridView.ExtensionLayers));
            }
        }

        public void IncrementState(CellPosition cellPosition)
        {
            int currentState = GetState(cellPosition);
            int newState = (currentState + 1) % 4;

            if (newState == 2 && stateMatrix.Any(t => t == 2))
                newState = 3;

            if (newState == 3 && stateMatrix.Any(t => t == 3))
                newState = 0;

            SetState(cellPosition, newState);
        }
    }

}