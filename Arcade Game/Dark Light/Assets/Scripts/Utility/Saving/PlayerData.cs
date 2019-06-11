using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------/
 * Script by Aiden Nathan.
 *---------------------------------*/

[System.Serializable]
public class PlayerData
{
    public int level;
    public int health;
    public float[] position = new float[3];

    public PlayerData(Player player) //Creates a reference for the Player and is used as the baseline for all data being saved into "save.dat".
    {
        //level = player.level;
        health = Player.curHealth;
        position[0] = Lamp.lPos[Lamp.lastSaved].position.x;
        position[1] = Lamp.lPos[Lamp.lastSaved].position.y;
        position[2] = Lamp.lPos[Lamp.lastSaved].position.z;

    }
}
