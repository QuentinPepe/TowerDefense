using UnityEngine;

namespace Grid
{
    public class GridView : MonoBehaviour
    {
        public GridModel GridModel { get; private set; }
        public GameObject[,] _cells;

        public void CreateGrid(int width, int height, float cellSize)
        {
            GridModel = new GridModel(width, height, cellSize);
            CreateGrid();
        }

        private void CreateGrid()
        {
            _cells = new GameObject[GridModel.Width, GridModel.Height];

            for (int x = 0; x < GridModel.Width; x++)
            {
                for (int y = 0; y < GridModel.Height; y++)
                {
                    GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cell.transform.position = GridModel.GetWorldPosition(x, y) + new Vector3(0.5f, 0, 0.5f) * GridModel.CellSize;
                    cell.transform.parent = transform;
                    cell.name = $"Cell_{x}_{y}";
                    cell.layer = LayerMask.NameToLayer("Ground");

                    _cells[x, y] = cell;
                }
            }
        }

        public GameObject GetCell(int x, int y)
        {
            if (x < 0 || x >= _cells.GetLength(0) || y < 0 || y >= _cells.GetLength(1)) return null;
            return _cells[x, y];
        }

    }
}