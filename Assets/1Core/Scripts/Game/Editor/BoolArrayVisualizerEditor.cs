using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoolArrayVisualizer))]
public class BoolArrayVisualizerEditor : Editor
{
    private BoolArrayVisualizer _target;

    private void OnEnable()
    {
        _target = (BoolArrayVisualizer)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty listProperty = serializedObject.FindProperty("list");

        EditorGUILayout.LabelField("Matrices:");

        // Draw the list of matrices
        for (int i = 0; i < listProperty.arraySize; i++)
        {
            SerializedProperty matrixProperty = listProperty.GetArrayElementAtIndex(i);
            SerializedProperty rowsProperty = matrixProperty.FindPropertyRelative("rows");
            SerializedProperty columnsProperty = matrixProperty.FindPropertyRelative("columns");
            SerializedProperty arrayProperty = matrixProperty.FindPropertyRelative("array");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Matrix {i + 1}:", GUILayout.MaxWidth(100));

            // Add button to remove the matrix
            if (GUILayout.Button("-", GUILayout.MaxWidth(30)))
            {
                listProperty.DeleteArrayElementAtIndex(i);
                break;
            }
            EditorGUILayout.EndHorizontal();

            // Display rows and columns for each matrix
            EditorGUILayout.PropertyField(rowsProperty);
            EditorGUILayout.PropertyField(columnsProperty);

            // Ensure the array has the correct size
            int rows = rowsProperty.intValue;
            int columns = columnsProperty.intValue;

            if (arrayProperty.arraySize != rows * columns)
            {
                arrayProperty.arraySize = rows * columns;
            }

            // Draw the matrix
            for (int r = 0; r < rows; r++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int c = 0; c < columns; c++)
                {
                    int index = r * columns + c;
                    SerializedProperty elementProperty = arrayProperty.GetArrayElementAtIndex(index);
                    elementProperty.boolValue = EditorGUILayout.Toggle(elementProperty.boolValue, GUILayout.Width(20));
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();
        }

        // Add button for adding new elements to the list
        if (GUILayout.Button("Add Matrix"))
        {
            listProperty.arraySize++;
            SerializedProperty newMatrix = listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);
            newMatrix.FindPropertyRelative("rows").intValue = 3;
            newMatrix.FindPropertyRelative("columns").intValue = 3;
            newMatrix.FindPropertyRelative("array").arraySize = 9;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
