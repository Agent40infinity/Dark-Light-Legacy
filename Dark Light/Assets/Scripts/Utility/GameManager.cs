using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Security;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static string loadedSave; //Variable that stores the loaded save.
    public static bool gameActive = false; //Is the game paused.
    public static AudioMixer masterMixer; //Creates reference for the menu music
    public static Dictionary<string, KeyCode> keybind = new Dictionary<string, KeyCode> //Dictionary to store the keybinds.
    {
        { "Up", KeyCode.W },
        { "Down", KeyCode.S },
        { "Left", KeyCode.A },
        { "Right", KeyCode.D },
        { "Jump", KeyCode.Space },
        { "Attack", KeyCode.E },
        { "Dash", KeyCode.LeftShift },
        { "Interact", KeyCode.F },
    };

    public void Start()
    {
        masterMixer = Resources.Load("Music/Mixers/Master") as AudioMixer; //Loads the MasterMixer for renference.

        if (File.Exists(Application.persistentDataPath + "/settings.json")) //Checks if the file already exists and loads the file if it does.
        {
            SystemConfig.LoadSettings();
        }
        else //Else, creates the data for the new file.
        {
            SystemConfig.SaveSettings(); //Saves the new data as a new file "Settings".
        }
    }
}
