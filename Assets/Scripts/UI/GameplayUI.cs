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
                decalRenderer.transform.position = hit.point + new Vector3(0, 0.75f, 0);
                if(Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Mouse clicked, do action");
                    DoPlayerAction(hit.point, selectedOption);
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

    }
}

public enum UIState
{
    DEFAULT,
    SELECTION
}
