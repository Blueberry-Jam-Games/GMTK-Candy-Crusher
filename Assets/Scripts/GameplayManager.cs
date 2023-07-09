using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [Header("Level config")]
    public List<Wave> waves;

    [Header("Runtime debug")]
    public int currentWave = 0;
    public int[] batalionCounts;
    public int availableRockets;

    public TowerAttackScript[] towers;

    public int sprinklesReload;
    public int peppermintReload;
    public int laserReload;

    void Start()
    {
        Wave initial = waves[0];
        batalionCounts = new int[initial.batalionReloads.Length];
        for(int i = 0; i < batalionCounts.Length; i++)
        {
            batalionCounts[i] = initial.batalionReloads[i];
        }
        availableRockets = initial.rocketReloads;

        towers = FindObjectsOfType<TowerAttackScript>();
    }

    private void Update()
    {
        // TODO check for game over and apply automatically
        bool batalionsAvailable = false;
        for(int i = 0; i < batalionCounts.Length; i++)
        {
            if(batalionCounts[i] != 0)
            {
                batalionsAvailable = true;
                break;
            }
        }
        if(!batalionsAvailable)
        {
            if(GameObject.FindGameObjectsWithTag("Soldier").Length == 0)
            {
                if(currentWave + 1 >= waves.Count)
                {
                    // Game Over
                }
                else
                {
                    // Encourage next level
                }
            }
            // Else let the soldiers finish
        }
        // Else gameplay is fine
    }

    public void NextWave()
    {
        Debug.Log("NextWave");
        currentWave++;
        if(currentWave > waves.Count)
        {
            // Game over
        }
        else
        {
            Wave newWave = waves[currentWave - 1];

            foreach(TowerAttackScript t in towers)
            {
                if(t.attackState == TowerType.SPRINKLES)
                {
                    t.ReloadAmmo(sprinklesReload);
                }
                else if(t.attackState == TowerType.PEPPERMINT)
                {
                    t.ReloadAmmo(peppermintReload);
                }
                else if(t.attackState == TowerType.LASER)
                {
                    t.ReloadAmmo(laserReload);
                }
            }

            for(int i = 0; i < batalionCounts.Length; i++)
            {
                batalionCounts[i] += newWave.batalionReloads[i];
            }
            availableRockets += newWave.rocketReloads;
        }
    }
}

[System.Serializable]
public class Wave
{
    public int[] batalionReloads;
    public int rocketReloads;
}

[System.Serializable]
public class TowerReload
{
    public TowerAttackScript target;
    public int reloadQuantity;
}