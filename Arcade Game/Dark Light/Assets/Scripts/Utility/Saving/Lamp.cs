using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------/
 * Script by Aiden Nathan.
 *---------------------------------*/

public class Lamp : MonoBehaviour
{
    public GameObject lamps;
    public Vector3 lParent;
    public Vector3[] lPos = new Vector3[3];

    public void Start()
    {
        lamps = GameObject.Find("Lamps");
        lParent = lamps.transform.position;
        //lPos = lamps.GetComponentInChildren<Transform>().position;
    }

}
