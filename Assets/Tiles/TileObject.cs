using UnityEngine;

public class TileObject : MonoBehaviour
{
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
        }
    }

}