using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class SettingData : MonoBehaviour
{
    public float masterMixer; //Variable for master volume.
    public float effectsMixer; //Variable for effects volume.
    public float musicMixer; //Variable for music volume.
    public float ambienceMixer;//Variable for ambience volume.

    public SettingData() //Used to set up the default data required for the json conversion.
    {
        float value;
        if (GameManager.masterMixer.GetFloat("Master", out value)) //If the mixer within MasterMixer exists, saves the value to it's respective variable so json conversion - Repeats 4 times for each Mixer.
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
    }
}
