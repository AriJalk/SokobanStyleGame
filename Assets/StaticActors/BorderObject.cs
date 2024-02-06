using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderObject : MonoBehaviour
{
    public BorderStruct BorderStruct { get; private set; }


    public void Initialize(BorderStruct borderStruct, Manager manager)
    {

        BorderStruct = borderStruct;
        TileObject tileA = borderStruct.TileA;
        TileObject tileB = borderStruct.TileB;

        transform.parent = manager.MapLayerTransform;
        Vector2Int difference = tileA.GridPosition - tileB.GridPosition;
        if (difference.x == 0)
        {
            transform.Rotate(Vector3.up * 90f);
        }
        Vector3 position = new Vector3();
        position.x = (tileA.transform.localPosition.x + tileB.transform.localPosition.x) / 2f;
        position.z = (tileA.transform.localPosition.z + tileB.transform.localPosition.z) / 2f;
        transform.localPosition = position;
        transform.localScale = Vector3.one;
    }
}
