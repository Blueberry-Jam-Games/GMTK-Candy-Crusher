using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumPlayer : MonoBehaviour
{
    float modifier = 1.0f;
    Vector2 targetPosition;
    Vector2 direction;

    public TowerGrid towerGrid;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 tempPos;
        tempPos.x = (int)transform.position.x;
        tempPos.y = transform.position.y;
        tempPos.z = (int)transform.position.z;

        targetPosition.x = tempPos.x;
        targetPosition.y =  tempPos.z;

        transform.position = tempPos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log("transform.position.x: " + transform.position.x + "targetPosition.x: " + targetPosition.x + "transform.position.z: " + transform.position.z + "targetPosition.y: " + targetPosition.y);
        if (0.02f > Math.Abs(targetPosition.x - transform.position.x) && 0.02f > Math.Abs(targetPosition.y - transform.position.z))
        {
            float x = targetPosition.x;
            float y = targetPosition.y;
            List<int> node = towerGrid.NextPos((int)x, (int)y);
            if (node[0] == -1 && node[1] == -1)
            {
                // Quinn needs to play an animation to destroy the player.
                Destroy(gameObject);
            }

            targetPosition.x = (float)(node[0]);
            targetPosition.y = (float)(node[1]);

            direction.x = (targetPosition.x - x);
            direction.y = targetPosition.y - y;
        }


        Vector3 tempPos = transform.position;
        if (direction.x != 0)
        {
            tempPos.x += direction.x * 0.02f * modifier;
        }
        else
        {
            tempPos.z += direction.y * 0.02f * modifier;
        }

        transform.position = tempPos;
    }
}
