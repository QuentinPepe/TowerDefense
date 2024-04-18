using Unity.AI.Navigation;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;

namespace Grid
{
    public class GridView : MonoBehaviour
    {
        public GridModel GridModel { get; private set; }
        private GameObject[,] _cells;

        [SerializeField] private NavMeshSurface navMeshSurface;
        [SerializeField] private GridState gridState;
        [SerializeField] private int width = 11;
        [SerializeField] private int height = 11;
        [SerializeField] private float cellSize = 1f;

        private void Awake()
        {
            gridState.InitializeMatrix(width, height);
            UpdateGrid();
        }

        public void OnValidate()
        {
            if (width < 1) width = 1;
            if (height < 1) height = 1;
            if (cellSize < 0.1f) cellSize = 0.1f;

            if (!Application.isPlaying)
            {
                EditorApplication.delayCall += UpdateGridWhenPossible;
            }

            gridState.InitializeMatrix(width, height);
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

                    NavMeshModifier navMeshModifier = cell.AddComponent<NavMeshModifier>();
                    navMeshModifier.overrideArea = true;
                    navMeshModifier.area = IsCellWalkable(x, y) ? 0 : 1;
                    navMeshModifier.ignoreFromBuild = !IsCellWalkable(x, y);

                    Renderer renderer = cell.GetComponent<Renderer>();
                    Material tempMaterial = new Material(renderer.sharedMaterial)
                    {
                        color = IsCellWalkable(x, y) ? Color.gray : Color.white
                    };
                    renderer.sharedMaterial = tempMaterial;

                    _cells[x, y] = cell;
                }
            }
            navMeshSurface.BuildNavMesh();
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
        public bool IsCellWalkable(int x, int y)
        {
            return gridState.GetState(x, y) == 1;
        }

        public GameObject GetCell(int x, int y)
        {
            if (x < 0 || x >= GridModel.Width || y < 0 || y >= GridModel.Height)
                return null;
            return _cells[x, y];
        }
    }
}