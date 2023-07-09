using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public void StartGame()
    {
        Debug.Log("Hello");
        StartCoroutine(LoadLevel("GrayboxTestQuinnConnor"));
    }

    IEnumerator LoadLevel(string scene)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(scene);
    }
}
