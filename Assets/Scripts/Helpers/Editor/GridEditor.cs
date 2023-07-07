using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridHelper))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GridHelper grid = (GridHelper)target;

        if (GUILayout.Button("Make Grid"))
        {
            grid.UpdateGrid();
        }
    }
}
