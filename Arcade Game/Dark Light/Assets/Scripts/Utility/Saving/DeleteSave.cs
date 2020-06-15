using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DeleteSave : MonoBehaviour
{
    public string ParentSave()
    {
        string parentSave;
        parentSave = gameObject.GetComponentInParent<GameSelection>().gameObject.name.Replace("Save", "");
        return parentSave;
    }

    public void Start()
    {
        fileCheck(gameObject);
    }

    public void fileCheck(GameObject delete)
    {
        string name = ParentSave();
        if (File.Exists(Application.persistentDataPath + "/save" + name + ".dat"))
        {
            delete.SetActive(true);
        }
        else
        {
            delete.SetActive(false);
        }
    }

    public void Delete()
    {
        string parentSave;
        parentSave = ParentSave();
        SystemSave.DeletePlayer(parentSave);
        fileCheck(gameObject);
    }
}