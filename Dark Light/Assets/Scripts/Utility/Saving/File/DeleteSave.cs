using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DeleteSave : MonoBehaviour
{
    public string ParentSave() //Used to figure out what save is being loaded
    {
        string parentSave;
        parentSave = gameObject.GetComponentInParent<GameSelection>().gameObject.name.Replace("Save", ""); //Replaces the name of the save with a number to use for loading
        return parentSave;
    }

    public void Awake() //Checks the file on start up.
    {
        fileCheck(gameObject);
    }

    public void fileCheck(GameObject delete) //Checks if the current save file exists.
    {
        string name = ParentSave(); //Gets the gameObject name.
        if (File.Exists(Application.persistentDataPath + "/save" + name + ".dat")) //Checks if the file exists in the directiory. If it exists, allow the file to be deleted, if it doesn't, do nothing.
        {
            delete.SetActive(true);
        }
        else
        {
            delete.SetActive(false);
        }
    }

    public void Delete() //Calls upon SystemSave to allow the player to delete a save file.
    {
        string parentSave;
        parentSave = ParentSave();
        SystemSave.DeletePlayer(parentSave);
        fileCheck(gameObject);
    }
}