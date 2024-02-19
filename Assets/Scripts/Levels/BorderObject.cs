using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: change to actor
public class BorderObject : MonoBehaviour
{
    public GameBorderStruct BorderStruct { get; private set; }


    //TODOL: 
    public void Initialize(GameBorderStruct borderStruct)
    {

        BorderStruct = borderStruct;

    }
}
