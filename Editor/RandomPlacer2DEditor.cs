using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomPlacer2D))]
public class RandomPlacer2DEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var placer = (RandomPlacer2D)target;

        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("HelpBox");
        EditorGUILayout.LabelField("Генерация в редакторе", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Расставить"))
        {
            placer.Place();
        }
        if (GUILayout.Button("Очистить"))
        {
            placer.Clear();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
}
