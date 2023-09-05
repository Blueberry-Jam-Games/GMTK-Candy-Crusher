using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMainMenuScript : MonoBehaviour
{
    public Button mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu.onClick.AddListener(GoToTitle);        
    }

    void GoToTitle()
    {
        LevelLoader levelLoader = FindObjectOfType<LevelLoader>();
        levelLoader.MainMenu();
    }
}
