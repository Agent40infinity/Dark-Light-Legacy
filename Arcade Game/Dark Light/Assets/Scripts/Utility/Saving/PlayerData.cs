using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------/
 * Script by Aiden Nathan.
 *---------------------------------*/

[System.Serializable]
public class PlayerData
{
    public bool dashUnlocked;
    public int lampIndex;
    public bool[] lampsLit;

    public PlayerData(Player player) //Creates a reference for the Player and is used as the baseline for all data being saved into "save.dat".
    {
        dashUnlocked = player.dashUnlocked;

        lampIndex = Lamp.lastSaved;
        lampsLit = new bool[Lamp.lLight.Length];
        for (int i = 0; i < Lamp.lLight.Length; i++)
        {
            lampsLit[i] = Lamp.lLight[i];
        }
    }
}
