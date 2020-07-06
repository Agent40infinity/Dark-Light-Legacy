using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Security;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static string loadedSave;
    public static bool gameActive = false;
    public static Dictionary<string, KeyCode> keybind = new Dictionary<string, KeyCode>();
    public static AudioMixer masterMixer; //Creates reference for the menu music

    public void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/settings.json"))
        {
            SystemSave.LoadSettings();
        }
        else
        {
            keybind.Add("Up", KeyCode.W);
            keybind.Add("Down", KeyCode.S);
            keybind.Add("Left", KeyCode.A);
            keybind.Add("Right", KeyCode.D);
            keybind.Add("Jump", KeyCode.Space);
            keybind.Add("Attack", KeyCode.E);
            keybind.Add("Dash", KeyCode.LeftShift);
            SystemSave.SaveSettings();
        }

    }
}
