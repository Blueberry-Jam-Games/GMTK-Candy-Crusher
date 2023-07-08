using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumPlayer : MonoBehaviour
{
    float modifier;
    Vector2 targetPosition;
    Vector2 direction;

    public TowerGrid towerGrid;

    bool damage = false;
    [SerializeField]
    private PlayerType state;
    [SerializeField]
    private float healthPoints = 100.0f;
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

        if (state == PlayerType.LARGE)
        {
            modifier = 0.01f;
        }
        else if (state == PlayerType.MEDIUM)
        {
            modifier = 0.02f;
        }
        else
        {
            modifier = 0.05f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (damage)
        {
            if (state == PlayerType.LARGE)
            {
                healthPoints -= 0.02f;
            }
            else if (state == PlayerType.MEDIUM)
            {
                healthPoints -= 0.05f;
            }
            else
            {
                healthPoints -= 0.1f;
            }
        }
        if (0.0f >= healthPoints)
        {
            Destroy(gameObject);
        }
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
            tempPos.x += direction.x * modifier;
        }
        else
        {
            tempPos.z += direction.y * modifier;
        }

        transform.position = tempPos;
    }

    public void SetDamage(bool val)
    {
        damage = val;
    }

    public bool GetDamage()
    {
        return damage;
    }
}

public enum PlayerType
{
    SMALL,
    MEDIUM,
    LARGE
}
