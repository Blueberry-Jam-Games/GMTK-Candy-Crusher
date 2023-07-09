using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialMaster : MonoBehaviour
{
    public delegate void OnComplete();

    public GameObject dark1;
    public GameObject dark2;
    public GameObject dark3;
    public GameObject dark4;
    public GameObject light1;
    public GameObject light2;

    public GameObject blackout;

    public TextMeshProUGUI text;
    public Image portrait;

    public RectTransform mouse;

    int state = 0;

    void Start()
    {
        StartCoroutine(DoDialogue(new List<string>(){
            "For years our cookies have been stolen from us. These men and women live in their ginger towns eating treats to no end.",
            "But no more, for it is you, General, who shall retrieve the cookies! Despite their best tower defenses, we shall march!"
        }, BattalionExample));
    }

    private IEnumerator DoDialogue(List<string> lines, OnComplete onComplete)
    {
        blackout.SetActive(true);

        int currentLine = 0;
        while(currentLine < lines.Count)
        {
            text.text = lines[currentLine];

            yield return new WaitForSeconds(0.5f);

            while(!Input.GetMouseButton(0))
            {
                yield return null;
            }
            currentLine++;
        }

        blackout.SetActive(false);
        text.text = string.Empty;
        onComplete?.Invoke();
    }

    public void BattalionExample()
    {

    }

    // private IEnumerator MoveMouse()
    // {
    //     mouse.position = new Vector3()
    // }
}
