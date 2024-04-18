using System;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEditor;

namespace Grid
{
    public class GridView : MonoBehaviour
    {
        public GridModel GridModel { get; private set; }
        private GameObject[,] _cells;
        private NavMeshSurface _navMeshSurface;

        [SerializeField] private int width = 11;
        [SerializeField] private int height = 11;
        [SerializeField] private float cellSize = 1f;

        private void Awake()
        {
            UpdateGrid();
        }

        private void OnValidate()
        {
            if (width < 1) width = 1;
            if (height < 1) height = 1;
            if (cellSize < 0.1f) cellSize = 0.1f;

            if (!Application.isPlaying)
            {
                EditorApplication.delayCall += UpdateGridWhenPossible;
            }

            if (_navMeshSurface == null)
                _navMeshSurface = GetComponent<NavMeshSurface>();
        }

        private void OnDestroy()
        {
            EditorApplication.delayCall -= UpdateGridWhenPossible;
        }

        private void UpdateGridWhenPossible()
        {
            if (this == null) return;
            EditorApplication.delayCall -= UpdateGridWhenPossible;
            UpdateGrid();
        }


        private void UpdateGrid()
        {
            ClearGrid();
            CreateGrid(width, height, cellSize);
        }

        private void CreateGrid(int width, int height, float cellSize)
        {
            GridModel = new GridModel(width, height, cellSize);
            BuildGrid();
        }

        private void BuildGrid()
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
            _navMeshSurface.BuildNavMesh();
        }

        private void ClearGrid()
        {
            Transform[] children = GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if (child != transform)
                    DestroyImmediate(child.gameObject);
            }
        }

        public GameObject GetCell(int x, int y)
        {
            if (x < 0 || x >= GridModel.Width || y < 0 || y >= GridModel.Height)
                return null;

            return _cells[x, y];
        }
    }
}