using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using Newtonsoft.Json;

public class SystemConfig : MonoBehaviour
{
    public static void SaveSettings() //Saves the settings data into a .json
    {
        string path = Application.persistentDataPath + "/settings.json"; //Gets the file in directory.
        SettingData settingData = new SettingData(SaveState.Save, null); //Creates a new SettingData so that the data can be serialised.

        string json = settingData.output; //temporarily saves the output to the json string.
        StreamWriter writer = File.CreateText(path); //Overrides/Creates a new file for settings based on the path and data provided.
        writer.Close();

        File.WriteAllText(path, json); //Saves the data to the .json using the json sring and path information.
        Debug.Log("Saved");
    }

    public static void LoadSettings() //Loads the settings data from the .json
    {
        string path = Application.persistentDataPath + "/settings.json"; //Gets the file in directory.
        string json = File.ReadAllText(path); //Reads the .json file under path and saves it to a temporary json string.
        string[] output = json.Split('|'); //Splits the data from the json string into an array of strings to allow the data to be loaded.
        SettingData settingData = new SettingData(SaveState.Load, output); //Creates a new SettingData so that the data can be deserialised and put back into the correct variables.
        Debug.Log("Loaded");
    }
}

public enum SaveState //Sets up save states to make switching easier
{
    Save,
    Load
}
