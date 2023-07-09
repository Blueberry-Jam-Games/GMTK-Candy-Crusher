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

    void Start()
    {
        //
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

            for(int i = 0; i < newWave.reloads.Count; i++)
            {
                TowerReload reload = newWave.reloads[i];
                if(reload != null)
                {
                    reload.target.ReloadAmmo(reload.reloadQuantity);
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
    public List<TowerReload> reloads;
    public int[] batalionReloads;
    public int rocketReloads;
}

[System.Serializable]
public class TowerReload
{
    public TowerAttackScript target;
    public int reloadQuantity;
}