#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class GridHelper : MonoBehaviour
{
    private TowerGrid parent;

    void Update()
    {
        if (parent == null)
        {
            parent = GetComponent<TowerGrid>();
        }        
    }

    public void OnDrawGizmos()
    {
        if (parent == null)
        {
            parent = GetComponent<TowerGrid>();
        }
        for (int x = 0; x < parent.grid.width; x++)
        {
            for (int y = 0; y < parent.grid.height; y++)
            {
                if(parent.grid.Get(x, y) == 0)
                {
                    Gizmos.color = new Color(0, 1, 0, 0.125f);
                }
                else
                {
                    Gizmos.color = new Color(1, 0, 0, 0.125f);
                }
                Vector3 gizmoSpot = new Vector3(x, 0.5f, y);
                Gizmos.DrawCube(gizmoSpot, Vector3.one);
                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(gizmoSpot, new Vector3(1, 0, 1));
            }
        }
    }

    public void UpdateGrid()
    {
        if (parent == null)
        {
            parent = GetComponent<TowerGrid>();
        }

        parent.DoGridRebuild();

        EditorUtility.SetDirty(parent.gameObject);
    }
}
#endif
