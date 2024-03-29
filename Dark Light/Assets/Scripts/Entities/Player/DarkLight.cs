﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------/
 * Script by Aiden Nathan.
 *---------------------------------*/

public class DarkLight : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player") //Checks whether or not the player has collected the DarkLight and acts accordingly.
        {
            Player.curMaxWisps = Player.maxWisps;
            Player.curWisps = Player.curMaxWisps;
            Destroy(this.gameObject);
        }
    }
}

