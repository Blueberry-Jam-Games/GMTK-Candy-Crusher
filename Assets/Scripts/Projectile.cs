using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 velocity;

    public float modifier = 0.001f;

    void Update()
    {
        float dx = Vector3.Dot(velocity, new Vector3(1.0f, 0.0f, 0.0f)) * modifier;
        float dy = 0.0f;//Vector3.Dot(velocity, new Vector3(0.0f, 1.0f, 0.0f)) * modifier;
        float dz = Vector3.Dot(velocity, new Vector3(0.0f, 0.0f, 1.0f)) * modifier;

        Vector3 tempPos = transform.position;
        tempPos.x += dx;
        tempPos.y += dy;
        tempPos.z += dz;

        transform.position = tempPos;
    }
}
