using UnityEditor;
using UnityEngine;

namespace Grid
{
    public class GridStateWindow : EditorWindow
    {
        private GridState _gridState;
        private readonly Vector2 _buttonSize = new Vector2(30, 30);
        private const float Padding = 10f;

        public static void ShowWindow(GridState currentState)
        {
            GridStateWindow window = GetWindow<GridStateWindow>("Grid State Editor");
            window._gridState = currentState;
            window.UpdateWindowSize();
        }

        private void UpdateWindowSize()
        {
            if (_gridState == null)
                return;

            float width = _gridState.Width * (_buttonSize.x + 0.5f) + Padding * 2 + 20;
            float height = _gridState.Height * (_buttonSize.y + 0.5f) + Padding * 3 + 40;
            minSize = new Vector2(width, height);
            maxSize = new Vector2(width, height);
        }

        private void OnGUI()
        {
            if (!_gridState)
            {
                EditorGUILayout.HelpBox("No Grid State selected.", MessageType.Warning);
                return;
            }

            if (_gridState.Width <= 0 || _gridState.Height <= 0)
            {
                EditorGUILayout.HelpBox("Invalid grid dimensions.", MessageType.Error);
                return;
            }

            GridController gridController = _gridState.GetComponent<GridController>();

            EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Space(Padding);
            try
            {
                for (int z = _gridState.Height - 1; z >= 0; z--)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int x = 0; x < _gridState.Width; x++)
                    {
                        CellPosition position = new CellPosition(x, z);
                        int currentState = _gridState.GetState(position);
                        GUI.color = currentState == 1 ? Color.white : Color.black;
                        if (!GUILayout.Button("", GUILayout.Width(_buttonSize.x), GUILayout.Height(_buttonSize.y))) continue;
                        _gridState.SetWalkable(position, currentState == 0);
                        EditorUtility.SetDirty(_gridState);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.Space(Padding);
                GUI.color = Color.white;
                if (!GUILayout.Button("Reset", GUILayout.Width(_buttonSize.x * _gridState.Width), GUILayout.Height(20))) return;
                _gridState.ResetMatrix();
                EditorUtility.SetDirty(_gridState);
            }
            finally
            {
                GUILayout.Space(Padding);
                EditorGUILayout.EndVertical();
            }
        }

        private void OnFocus()
        {
            UpdateWindowSize();
        }
    }
}