﻿using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Grid
{
    public class GridStateWindow : EditorWindow
    {
        private GridState _gridState;
        private readonly Vector2 _buttonSize = new Vector2(30, 30);
        private const float Padding = 10f;
        private Vector2 _scrollPosition;

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

            float width = Mathf.Max(600, _gridState.Width * (_buttonSize.x + 0.5f) + Padding * 2);
            float height = Mathf.Max(400, _gridState.Height * (_buttonSize.y + 0.5f) + Padding * 3 + 40);
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

            EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Space(Padding);

            // Scroll view begins here
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(minSize.x), GUILayout.Height(minSize.y - 50));
            try
            {
                for (int z = _gridState.Height - 1; z >= 0; z--)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int x = 0; x < _gridState.Width; x++)
                    {
                        CellPosition position = new CellPosition(x + GridView.ExtensionLayers, z + GridView.ExtensionLayers);
                        int currentState = _gridState.GetState(position);

                        Color color = currentState switch
                        {
                            0 => Color.white,
                            1 => Color.black,
                            2 => Color.green,
                            3 => Color.red,
                            _ => throw new System.ArgumentOutOfRangeException()
                        };

                        GUI.color = color;
                        if (GUILayout.Button("", GUILayout.Width(_buttonSize.x), GUILayout.Height(_buttonSize.y)))
                        {
                            _gridState.IncrementState(position);
                            EditorUtility.SetDirty(_gridState);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            finally
            {
                EditorGUILayout.EndScrollView();
                GUILayout.Space(Padding);
                EditorGUILayout.EndVertical();
            }

            GUILayout.Space(Padding);
            GUI.color = Color.white;
            if (GUILayout.Button("Reset", GUILayout.Width(_buttonSize.x * _gridState.Width), GUILayout.Height(20)))
            {
                _gridState.ResetMatrix();
                EditorUtility.SetDirty(_gridState);
            }
        }

        private void OnFocus()
        {
            UpdateWindowSize();
        }
    }
}