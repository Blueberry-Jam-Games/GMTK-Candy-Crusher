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

    private Animator animator;
    public SpriteRenderer localRenderer;
    private string currentAnimation = string.Empty;

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

        localRenderer.sharedMaterial = new Material(localRenderer.sharedMaterial);
        this.animator = GetComponent<Animator>();

        if (state == PlayerType.LARGE)
        {
            modifier = 0.01f;
            animator.Play("DefaultJawbreaker");
            currentAnimation = "DefaultJawbreaker";
        }
        else if (state == PlayerType.MEDIUM)
        {
            modifier = 0.02f;
            localRenderer.sharedMaterial.SetFloat("_HueShift", 0f);
            animator.Play("Default");
            currentAnimation = "Default";
        }
        else if (state == PlayerType.SMALL)
        {
            modifier = 0.05f;
            localRenderer.sharedMaterial.SetFloat("_HueShift", 210f);
            animator.Play("Default");
        }

        this.id = GLOBAL_ID++;
    }

    void FixedUpdate()
    {
        //Debug.Log("transform.position.x: " + transform.position.x + "targetPosition.x: " + targetPosition.x + "transform.position.z: " + transform.position.z + "targetPosition.y: " + targetPosition.y);
        if (0.1f > Math.Abs(targetPosition.x - transform.position.x) && 0.1f > Math.Abs(targetPosition.y - transform.position.z))
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

            // Debug.Log("Got direction feedback " + targetPosition);
            string animation = string.Empty;
            localRenderer.flipX = false;
            if(direction.y > 0)
            {
                // Debug.Log("Y > 0 backwards");
                if(state == PlayerType.LARGE)
                {
                    animation = "JawbreakerWalkBackward";
                }
                else
                {
                    animation = "DefaultWalkBack"; 
                }
            }
            else if(direction.y < 0)
            {
                // Debug.Log("Y < 0 Forwards");
                if(state == PlayerType.LARGE)
                {
                    animation = "JawbreakerWalkForward";
                }
                else
                {
                    animation = "DefaultWalkForward"; 
                }
            }
            else if(direction.x > 0)
            {
                // Debug.Log("X > 0 right");
                if(state == PlayerType.LARGE)
                {
                    animation = "JawbreakerWalkRight";
                }
                else
                {
                    animation = "DefaultWalkRight"; 
                }
            }
            else if(direction.x < 0)
            {
                // Debug.Log("X < 0 left");
                localRenderer.flipX = true;
                if(state == PlayerType.LARGE)
                {
                    animation = "JawbreakerWalkRight";
                }
                else
                {
                    animation = "DefaultWalkRight"; 
                }
            }
            else
            {
                // Debug.Log("XY = 0 Default");
                if(state == PlayerType.LARGE)
                {
                    animation = "DefaultJawbreaker";
                }
                else
                {
                    animation = "Default"; 
                }
            }
            
            // Debug.Log($"Current Anim {currentAnimation} new anim {animation} deciding");

            if(animation != currentAnimation)
            {
                // Debug.Log("Change Animation");
                int frameOffset = (Time.frameCount % 40);
                float animationOffset = frameOffset / 40;
                animationOffset += UnityEngine.Random.Range(0f, 1f/60f) % 1f;
                animator.Play(animation, -1, animationOffset);
                currentAnimation = animation;
            }
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

        float height = Terrain.activeTerrain.SampleHeight(new Vector3(tempPos.x, 0, tempPos.z)) + 0.5f;
        tempPos.y = height;

        transform.position = tempPos;
    }

    public int Sign(float f)
    {
        return f == 0 ? 0 : (f < 0) ? -1 : 1;
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
