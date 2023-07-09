using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListScenes : MonoBehaviour
{
    public string[] levels;
    public int currentLevel;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
