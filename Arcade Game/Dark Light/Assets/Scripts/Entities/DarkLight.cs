using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------/
 * Script by Aiden Nathan.
 *---------------------------------*/

public class DarkLight : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {

            Destroy(this.gameObject);
        }
    }
}

