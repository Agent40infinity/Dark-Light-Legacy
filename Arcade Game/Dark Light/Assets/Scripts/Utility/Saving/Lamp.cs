using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------/
 * Script by Aiden Nathan.
 *---------------------------------*/

public class Lamp : MonoBehaviour
{
    public GameObject lamps;
    public Transform[] lPos;

    public void Start()
    {
        lamps = GameObject.Find("Lamps");
        lPos = lamps.GetComponentsInChildren<Transform>();
    }

}
