using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayUI : MonoBehaviour
{
    [Header("Config")]
    public Vector3 arrowRoot;

    [Header("Internal Refs")]
    public Button batalion1;
    public TextMeshProUGUI batalionQty1;

    public GameObject mediumPlayer;
    public GameObject decalRenderer;

    public UIState state = UIState.DEFAULT;

    private int selectedOption; // 1-3 = Batalion, 4 = rocket

    private void Start()
    {
        decalRenderer.SetActive(false);
        batalion1.onClick.AddListener(OnBatalion1Press);
    }

    private void Update()
    {
        if (state == UIState.SELECTION)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                // Debug.Log("Point was " + hit.point);
                decalRenderer.transform.position = hit.point + new Vector3(0, 2f, 0);
                if(Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Mouse clicked, do action");
                    Vector3 tempPos;
                    tempPos.x = Mathf.Floor(hit.point.x);
                    tempPos.y = Mathf.Floor(hit.point.y);
                    tempPos.z = Mathf.Floor(hit.point.z);
                    if (tempPos.z == 0.0f)
                    {
                        DoPlayerAction(tempPos, selectedOption);
                    }
                    state = UIState.DEFAULT;

                    decalRenderer.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("Didn't hit, this shouldn't be doable");
                Debug.DrawLine(arrowRoot, ray.direction * 100);
            }
        }
    }

    public void OnBatalion1Press()
    {
        OnBatalionButtonPressed(1);
    }

    private void OnBatalionButtonPressed(int batalion)
    {
        if (state == UIState.DEFAULT)
        {
            state = UIState.SELECTION;
            selectedOption = batalion;
            decalRenderer.SetActive(true);
        }
    }

    private void DoPlayerAction(Vector3 target, int type)
    {
        StartCoroutine(SpawnBattalion(target, type));
    }

    private IEnumerator SpawnBattalion(Vector3 target, int type)
    {
        int end = 0;
        if(type == 1)
        {
            end = 20;
        }
        else if(type == 2)
        {
            end = 5;
        }
        else if(type == 3)
        {
            end = 2;
        }
        for (int i = 0; i < end; i++)
        {
            MediumPlayer player = Instantiate(mediumPlayer, target, Quaternion.Euler(0,0,0)).GetComponent<MediumPlayer>();
            if(type == 1)
            {
                player.state = PlayerType.SMALL;
            }
            else if(type == 2)
            {
                player.state = PlayerType.MEDIUM;
            }
            else if(type == 3)
            {
                player.state = PlayerType.LARGE;
            }
            yield return new WaitForSeconds(0.16f);
        }
    }
}

public enum UIState
{
    DEFAULT,
    SELECTION
}
