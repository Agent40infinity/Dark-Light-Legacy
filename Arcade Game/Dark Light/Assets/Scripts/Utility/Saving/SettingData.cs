using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using JetBrains.Annotations;

[System.Serializable]
public class SettingData : MonoBehaviour
{
    public string keybind;
    public float masterMixer;
    public float effectsMixer;
    public float musicMixer;
    public float ambienceMixer;

    public SettingData()
    {
        keybind = JsonConvert.SerializeObject(GameManager.keybind, Formatting.Indented);
        float value;
        if (GameManager.masterMixer.GetFloat("Master", out value))
        { 
            masterMixer = value; 
        }
        if (GameManager.masterMixer.GetFloat("Effects", out value))
        { 
            effectsMixer = value; 
        }
        if (GameManager.masterMixer.GetFloat("Music", out value))
        { 
            musicMixer = value; 
        }
        if (GameManager.masterMixer.GetFloat("Ambience", out value))
        { 
            ambienceMixer = value; 
        }

        Debug.Log(keybind);
    }
}
