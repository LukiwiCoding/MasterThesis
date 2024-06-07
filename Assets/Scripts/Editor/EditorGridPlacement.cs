using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

[CustomEditor(typeof(GridManager))]
public class EditorGridPlacement : Editor
{
#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        GridManager gridManager = (GridManager)target;
        if(GUILayout.Button("Generate Grid"))
        {
            gridManager.GenerateGrid();
        }
        if(GUI.changed) EditorUtility.SetDirty(target);

        DrawDefaultInspector();
    }
#endif
}
