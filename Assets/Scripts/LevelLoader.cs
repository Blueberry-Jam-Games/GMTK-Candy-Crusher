using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    private bool enterPressed;

    void Start()
    {
        enterPressed = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && enterPressed && SceneManager.GetActiveScene().name == "TitleScreen")
        {
            Debug.Log("Code playing");
            enterPressed = false;
            ListScenes gameplay = FindObjectOfType<ListScenes>();
            gameplay.currentLevel++;
            StartCoroutine(LoadLevel("TutorialScene"));
        }
    }

    public void findNextLevel()
    {
        GameObject winScreen = GameObject.FindWithTag("WinScreen");
        winScreen.GetComponent<Canvas>().enabled = false;
        ListScenes gameplay = FindObjectOfType<ListScenes>();
        if (gameplay.levels.Length > gameplay.currentLevel + 1)
        {
            gameplay.currentLevel++;
            StartCoroutine(LoadLevel(gameplay.levels[gameplay.currentLevel]));
        }
        else
        {
            gameplay.currentLevel = 0;
        }
    }

    public void MainMenu()
    {
        GameObject winScreen = GameObject.FindWithTag("WinScreen");
        winScreen.GetComponent<Canvas>().enabled = false;
        ListScenes gameplay = FindObjectOfType<ListScenes>();
        gameplay.currentLevel = 0;
        StartCoroutine(LoadLevel(gameplay.levels[gameplay.currentLevel]));
    }

    public IEnumerator LoadLevel(string scene)
    {
        transition.Play("ingerRunningg");
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadSceneAsync(scene);
        yield return new WaitForSeconds(0.6f);
        // //
    }
}
