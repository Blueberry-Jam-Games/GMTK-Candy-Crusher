using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BlockingObject : MonoBehaviour
{
    public int width = 1;
    public int height = 1;

    public float xOffset = 0;
    public float zOffset = 0;

    public bool lockToGrid = false;

    public int GetX()
    {
        return Mathf.RoundToInt(transform.position.x + xOffset);
    }

    public int GetZ()
    {
        return Mathf.RoundToInt(transform.position.z + zOffset);
    }

    public void SnapToGrid()
    {
        Vector3 newTransform = new Vector3(Mathf.RoundToInt(transform.position.x) + xOffset, transform.position.y, Mathf.RoundToInt(transform.position.z) + zOffset);
        transform.position = newTransform;
        #if UNITY_EDITOR
        EditorUtility.SetDirty(this.gameObject);
        #endif
    }

    public virtual void UpdateGeometry()
    {

    }
}
