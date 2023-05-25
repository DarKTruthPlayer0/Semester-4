using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueOrganizer))]
public class DialogueOrganizerEditor : Editor
{
    SerializedProperty listProperty;
    SerializedProperty selectedElementProperty;

    private void OnEnable()
    {
        listProperty = serializedObject.FindProperty("DialogueList");
        selectedElementProperty = serializedObject.FindProperty("SelectedElement");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(listProperty, true);

        Dictionary<string, int> elementIndexMap = new Dictionary<string, int>();
        string[] options = new string[listProperty.arraySize];
        for (int i = 0; i <  listProperty.arraySize; i++)
        {
            string element = listProperty.GetArrayElementAtIndex(i).stringValue;
            options[i] = element;

            elementIndexMap[element] = i;
        }

        if (elementIndexMap.ContainsKey(selectedElementProperty.stringValue))
        {
            int selectedIndex = EditorGUILayout.Popup("SelectedElement", elementIndexMap[selectedElementProperty.stringValue], options);

            selectedElementProperty.stringValue = options[selectedIndex];
        }
        else
        {
            if (options.Length > 0)
            {
                selectedElementProperty.stringValue = options[0];
                EditorGUILayout.Popup("SelectedElement", 0, options);
            }
            else
            {
                selectedElementProperty.stringValue = string.Empty;
                EditorGUILayout.Popup("SelectedElement", 0, new string[] { "None" });
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
