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
            StartCoroutine(LoadLevel("GrayboxTestQuinnConnor"));
        }
    }

    IEnumerator LoadLevel(string scene)
    {
        transition.Play("ingerRunningg");
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadSceneAsync(scene);
        yield return new WaitForSeconds(0.6f);
        // //
    }
}
