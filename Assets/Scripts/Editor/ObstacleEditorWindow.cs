using UnityEditor;
using UnityEngine;

public class ObstacleEditorWindow : EditorWindow {
    private ObstacleData_SO targetData;

    // Creates a "Grid/ Obstacle Editor" at Unity's top menu bar
    [MenuItem("Grid/ Obstacle Editor")]
    public static void ShowWindow() {
        GetWindow<ObstacleEditorWindow>("Obstacle Editor");
    }

    private void OnGUI() {

        // Accepts any ObstacleData asset that the user wants to edit
        targetData = (ObstacleData_SO)EditorGUILayout.ObjectField(
            "Obstacle Data", targetData, typeof(ObstacleData_SO), false);
        
        // Shows a message if the targetData field is empty
        if (targetData == null) {
            EditorGUILayout.HelpBox("Assign an ObstacleData asset to edit.", MessageType.Info);
            return;
        }

        SerializedObject serializedData = new SerializedObject(targetData);
        SerializedProperty blockedArray = serializedData.FindProperty("blockedTiles");

        EditorGUILayout.Space(10);

        // Draw rows from top (y=9) to bottom (y=0), visually matching the grid orientation
        for (int y = ObstacleData_SO.GRID_HEIGHT - 1; y >= 0; y--) {
            EditorGUILayout.BeginHorizontal();

            for (int x = 0; x < ObstacleData_SO.GRID_WIDTH; x++) {
                int index = x + y * ObstacleData_SO.GRID_WIDTH;
                SerializedProperty tileProp = blockedArray.GetArrayElementAtIndex(index);

                // Set the button color to red if blocked
                Color originalColor = GUI.backgroundColor;
                GUI.backgroundColor = tileProp.boolValue ? Color.red : Color.white;

                // A fixed-size button labeled with its coordinates
                if (GUILayout.Button($"{x},{y}", GUILayout.Width(40), GUILayout.Height(40))) {
                    tileProp.boolValue = !tileProp.boolValue; // toggle on click
                }

                GUI.backgroundColor = originalColor; // restore so it doesn't bleed into other GUI
            }

            EditorGUILayout.EndHorizontal();
        }

        // Commit changes back to the actual asset, with Undo support
        serializedData.ApplyModifiedProperties();
    }
}
