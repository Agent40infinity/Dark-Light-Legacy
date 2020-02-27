using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameSelection : MonoBehaviour
{
    Player player;
    Menu menu;

    public void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        menu = GameObject.FindWithTag("Menu").GetComponent<Menu>();
    }

    public void CreateLoad()
    {
        string name = gameObject.name.Replace("Save", "");
        Debug.Log(Application.persistentDataPath);
        if (File.Exists("/save" + name + ".dat"))
        {
            name = GameManager.loadedSave;
            SystemSave.LoadPlayer(player, name);
        }
        else
        {
            SystemSave.SavePlayer(player, name);
            Debug.Log("Created File");
            menu.StartGame();
        }
    }
}
