using UnityEditor;
using UnityEngine;

namespace Grid
{
    [CustomEditor(typeof(GridState))]
    public class GridStateInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // Draws the default inspector

            // Check if multiple objects are selected
            if (serializedObject.isEditingMultipleObjects)
            {
                EditorGUILayout.HelpBox("Multi-object editing is not supported.", MessageType.Info);
            }
            else
            {
                if (GUILayout.Button("Open Grid State Editor"))
                {
                    GridStateWindow.ShowWindow((GridState)target);
                }
            }
        }
    }
}