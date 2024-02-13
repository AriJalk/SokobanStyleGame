using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    public Camera Camera;

    public Transform Raycast(Vector2 position, LayerMask layerMask)
    {
        Ray ray = Camera.ScreenPointToRay(position);
        return Raycast(ray, layerMask);
    }

    public Transform Raycast(Ray ray, LayerMask layerMask)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20f, layerMask))
        {
            Debug.Log(hit.transform.name);
            return hit.transform;
        }
        return null;
    }

}
