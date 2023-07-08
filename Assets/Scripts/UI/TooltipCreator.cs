using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipCreator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string message;

    public Vector3 offset;

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log("Pointer Enter");
        if (Tooltip.Initialized)
        {
            Tooltip.RequestTooltip(message, transform.position + offset, this.gameObject);
        }
        else
        {
            Debug.LogError("Tooltip not initialized. Add TooltipHome Prefab to the scene");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("Pointer exit");
        if (Tooltip.Initialized)
        {
            Tooltip.RequestHideTooltip();
        }
        else
        {
            Debug.LogError("Tooltip not initialized. Add TooltipHome Prefab to the scene");
        }
    }
}
