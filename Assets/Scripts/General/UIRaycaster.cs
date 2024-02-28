using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UIRaycaster
{

    // Perform a UI raycast using screen coordinates
    public static Transform Raycast(Vector2 position, LayerMask layerMask)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = position;

        // Raycast against all UI elements at the given screen position
        List<RaycastResult> raycastResults = new List<RaycastResult>(); // Adjust the size as needed
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        // Find the first valid raycast result on the specified layer
        foreach (RaycastResult result in raycastResults)
        {
            if (result.isValid && result.gameObject != null)
            {
                // Check if the hit object is on the specified layer
                if ((layerMask & (1 << result.gameObject.layer)) != 0)
                {
                    //Debug.Log(result.gameObject.name);
                    return result.gameObject.transform;
                }
            }
        }

        return null;
    }

}
