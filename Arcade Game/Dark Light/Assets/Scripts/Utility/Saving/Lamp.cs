using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------/
 * Script by Aiden Nathan.
 *---------------------------------*/

public class Lamp : MonoBehaviour
{
    public GameObject lamps;
    public static int lastSaved;
    public static Transform[] lPos;

    public void Start()
    {
        lastSaved = 1;
        lamps = GameObject.Find("Lamps");
        lPos = lamps.GetComponentsInChildren<Transform>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            
        }
    }
}
