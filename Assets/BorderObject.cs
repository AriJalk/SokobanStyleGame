using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: change to actor
public class BorderObject : MonoBehaviour
{
    public BorderStruct BorderStruct { get; private set; }


    //TODOL: 
    public void Initialize(BorderStruct borderStruct)
    {

        BorderStruct = borderStruct;

    }
}
