using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static string loadedSave;
    public static bool gameActive = false;
    public static Dictionary<string, KeyCode> keybind = new Dictionary<string, KeyCode>();

    public void Awake()
    {
        keybind.Add("Up", KeyCode.W);
        keybind.Add("Down", KeyCode.S);
        keybind.Add("Left", KeyCode.A);
        keybind.Add("Right", KeyCode.D);
        keybind.Add("Jump", KeyCode.Space);
        keybind.Add("Attack", KeyCode.E);
        keybind.Add("Dash", KeyCode.LeftShift);
    }
}
