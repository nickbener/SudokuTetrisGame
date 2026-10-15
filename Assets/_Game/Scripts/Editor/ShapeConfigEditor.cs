using UnityEditor;
using UnityEngine;

 [CustomEditor(typeof(ShapeConfigSO))]
public class ShapeConfigEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ShapeConfigSO shapeConfig = (ShapeConfigSO)target;
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Редактор", EditorStyles.boldLabel);
        for (int y = 4; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            for (int x = 0; x < 5; x++)
            {
                 bool currentState = shapeConfig.GetCellActive(x, y);
                 GUI.backgroundColor = currentState ? Color.green : Color.lightGray;

                 if (GUILayout.Button("", GUILayout.Width(35), GUILayout.Height(35)))
                 {
                     Undo.RecordObject(shapeConfig, "Toggle Shape Cell");
                     shapeConfig.SetCellActive(x, y, !currentState);
                     EditorUtility.SetDirty(shapeConfig);
                }
                GUI.backgroundColor = Color.white;
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
}