using UnityEngine;

public class TileObject : MonoBehaviour
{
    public Transform ObjectSlotTransform;
    public Vector2Int GridPosition { get; set; }
    public TileType TileType { get; set; }
    private GameObject _objectSlot;
    public GameObject ObjectSlot
    {
        get
        {
            return _objectSlot;
        }
        set
        {
            _objectSlot = value;
            _objectSlot.transform.SetParent(ObjectSlotTransform, false);
        }
    }

}