using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------/
 * Script by Aiden Nathan.
 *---------------------------------*/

[System.Serializable]
public class PlayerData
{
    public int lampIndex;
    public bool[] lampsLit;
    public Dictionary<string, bool> abilitiesUnlocked = new Dictionary<string, bool>();

    public PlayerData(Player player) //Creates a reference for the Player and is used as the baseline for all data being saved into "save.dat".
    {
        lampIndex = Lamp.lastSaved;
        lampsLit = new bool[Lamp.lLight.Length];
        for (int i = 0; i < Lamp.lLight.Length; i++)
        {
            lampsLit[i] = Lamp.lLight[i];
        }

        abilitiesUnlocked = Player.abilitiesUnlocked;
    }

    public void LoadData(Player player)
    {
        GameObject[] lampControllers = GameObject.FindGameObjectsWithTag("Save");
        for (int i = 0; i < lampsLit.Length; i++)
        {
            if (lampsLit[i] && lampControllers[i].GetComponent<LampController>())
            {
                lampControllers[i].GetComponent<LampController>().LoadLamp();
            }
        }
        Lamp.lastSaved = lampIndex;

        Player.abilitiesUnlocked = abilitiesUnlocked;

        Vector3 playerPos = new Vector3(Lamp.lPos[Lamp.lastSaved].position.x, Lamp.lPos[Lamp.lastSaved].position.y, 0);
        player.transform.position = playerPos;
    }
}
