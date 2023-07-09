using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumPlayer : MonoBehaviour
{
    float modifier;
    Vector2 targetPosition;
    Vector2 direction;

    private TowerGrid towerGrid;

    [SerializeField]
    public PlayerType state;

    [SerializeField]
    public float healthPoints;

    public int id;

    public static int GLOBAL_ID = 0;

    void Start()
    {
        towerGrid = GameObject.FindGameObjectWithTag("TowerGrid").GetComponent<TowerGrid>();
        
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

        this.id = GLOBAL_ID++;
    }

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
            tempPos.x += direction.x * modifier;
        }
        else
        {
            tempPos.z += direction.y * modifier;
        }

        transform.position = tempPos;
    }

    public void DoDamage(float ammount)
    {
        healthPoints -= ammount;
        if(healthPoints <= 0f)
        {
            Destroy(this.gameObject);
        }
    }

    public void SlowDown(float amount)
    {
        modifier = modifier * amount;
    }
}

public enum PlayerType
{
    SMALL,
    MEDIUM,
    LARGE
}
