using UnityEngine;

namespace Grid
{
    public class GridModel
    {
        public int Width { get; }
        public int Height { get; }
        public float CellSize { get; }

        public GridModel(int width, int height, float cellSize)
        {
            Width = width;
            Height = height;
            CellSize = cellSize;
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x * CellSize, 0f, y * CellSize);
        }

        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(worldPosition.x / CellSize);
            y = Mathf.FloorToInt(worldPosition.z / CellSize);
        }
    }
}