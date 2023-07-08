using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitObject : MonoBehaviour
{
    public int width = 1;
    public int height = 1;

    public float xOffset = 0;
    public float zOffset = 0;

    public int GetX()
    {
        return Mathf.RoundToInt(transform.position.x + xOffset);
    }

    public int GetZ()
    {
        return Mathf.RoundToInt(transform.position.z + zOffset);
    }
}
