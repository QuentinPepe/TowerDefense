using Unity.AI.Navigation;
using UnityEngine;
using UnityEditor;

namespace Grid
{
    public class GridView : MonoBehaviour
    {
        public GridModel GridModel { get; private set; }
        private GameObject[,] _cells;
        [SerializeField] private BoxCollider boxCollider;

        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private GameObject walkablePrefab;

        [SerializeField] private NavMeshSurface navMeshSurface;
        [SerializeField] private GridController gridController;
        [SerializeField] private GridState gridState;
        [SerializeField] private int width = 11;
        [SerializeField] private int height = 11;
        [SerializeField] private float cellSize = 1f;
        [SerializeField] private Transform cameraTarget;

        public const int ExtensionLayers = 15;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();
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
        }

        private void UpdateGridWhenPossible()
        {
            if (this == null) return;
            gridState.InitializeMatrix(width, height);
            EditorApplication.delayCall -= UpdateGridWhenPossible;
            UpdateGrid();
        }

        public void UpdateGrid()
        {
            ClearGrid();
            CreateGrid(width + 2 * ExtensionLayers, height + 2 * ExtensionLayers, cellSize);
            GenerateConfinementArea();

            Vector3 gridSize = new Vector3(width * cellSize, 40, height * cellSize);
            cameraTarget.position = new Vector3(ExtensionLayers * cellSize + gridSize.x / 2, 0, ExtensionLayers * cellSize + gridSize.z / 2);
        }

        private void CreateGrid(int totalWidth, int totalHeight, float cellSize)
        {
            GridModel = new GridModel(totalWidth, totalHeight, cellSize);
            BuildGrid();
        }

        private void BuildGrid()
        {
            _cells = new GameObject[GridModel.Width, GridModel.Height];
            for (int x = 0; x < GridModel.Width; x++)
            {
                for (int y = 0; y < GridModel.Height; y++)
                {
                    bool isExtension = x < ExtensionLayers || x >= GridModel.Width - ExtensionLayers || y < ExtensionLayers || y >= GridModel.Height - ExtensionLayers;
                    GameObject prefab = isExtension ? floorPrefab : (gridController.IsCellWalkable(new CellPosition(x, y)) ? walkablePrefab : floorPrefab);

                    GameObject cell = Instantiate(prefab, GridModel.GetWorldPosition(x, y) + new Vector3(0.5f, 0, 0.5f) * GridModel.CellSize, Quaternion.identity, transform);

                    cell.transform.position = GridModel.GetWorldPosition(x, y) + new Vector3(0.5f, 0, 0.5f) * GridModel.CellSize;
                    cell.transform.parent = transform;
                    cell.name = $"Cell_{x}_{y}";
                    if (isExtension) continue;
                    cell.layer = LayerMask.NameToLayer("Ground");
                    NavMeshModifier navMeshModifier = cell.AddComponent<NavMeshModifier>();
                    navMeshModifier.overrideArea = true;

                    CellPosition cellPosition = new CellPosition(x, y);
                    navMeshModifier.area = gridController.IsCellWalkable(cellPosition) ? 0 : 1;
                    navMeshModifier.ignoreFromBuild = !gridController.IsCellWalkable(cellPosition);

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

        public GameObject GetCell(CellPosition position)
        {
            if (position.X < 0 || position.X >= GridModel.Width || position.Z < 0 || position.Z >= GridModel.Height)
                return null;
            return _cells[position.X, position.Z];
        }

        private void GenerateConfinementArea()
        {
            Vector3 gridSize = new Vector3(width * cellSize, 40, height * cellSize);

            boxCollider.size = gridSize;
            boxCollider.center = new Vector3(ExtensionLayers * cellSize + gridSize.x / 2, 20, ExtensionLayers * cellSize + gridSize.z / 2);
        }
    }
}