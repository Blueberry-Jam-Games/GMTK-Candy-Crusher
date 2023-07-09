using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tooltipText;
    private GameObject owner;

    private static Tooltip Instance;
    public static bool Initialized
    {
        get => Instance != null;
    }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.parent.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        gameObject.SetActive(false);
    }

    private void ShowToolTip(string tooltipString, Vector3 position, GameObject newOwner)
    {
        if(newOwner == owner || owner != null)
        {
            // Duplicate request
            return;
        }
        gameObject.SetActive(true);

        transform.position = position;

        tooltipText.text = tooltipString;
    }

    private void HideTooltip()
    {
        owner = null;
        tooltipText.text = string.Empty;
        gameObject.SetActive(false);
    }

    public static void RequestTooltip(string message, Vector3 position, GameObject newOwner)
    {
        Instance.ShowToolTip(message, position, newOwner);
    }

    public static void RequestHideTooltip()
    {
        Instance.HideTooltip();
    }
}
