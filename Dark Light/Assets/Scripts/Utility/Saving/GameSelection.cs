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
        Debug.Log(File.Exists(Application.persistentDataPath + "/save" + name + ".dat"));
        GameManager.loadedSave = name;
        if (File.Exists(Application.persistentDataPath + "/save" + name + ".dat"))
        {
            SystemSave.LoadPlayer(player, name);
        }
        else
        {
            SystemSave.SavePlayer(player, name);
            Debug.Log("Created File");
        }
        menu.StartGame();
    }
}