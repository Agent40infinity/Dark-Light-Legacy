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
    public static Dictionary<string, KeyCode> keybind = new Dictionary<string, KeyCode>(); //Dictionary to store the keybinds.
    public static AudioMixer masterMixer; //Creates reference for the menu music

    public void Start()
    {
        masterMixer = Resources.Load("Music/Mixers/Master") as AudioMixer; //Loads the MasterMixer for renference.

        keybind.Add("Up", KeyCode.W);
        keybind.Add("Down", KeyCode.S);
        keybind.Add("Left", KeyCode.A);
        keybind.Add("Right", KeyCode.D);
        keybind.Add("Jump", KeyCode.Space);
        keybind.Add("Attack", KeyCode.E);
        keybind.Add("Dash", KeyCode.LeftShift);
        keybind.Add("Interact", KeyCode.F);

        if (File.Exists(Application.persistentDataPath + "/settings.json")) //Checks if the file already exists and loads the file if it does.
        {
            SystemSave.LoadSettings();
        }
        else //Else, creates the data for the new file.
        {
            SystemSave.SaveSettings(); //Saves the new data as a new file "Settings".
        }
    }
}
