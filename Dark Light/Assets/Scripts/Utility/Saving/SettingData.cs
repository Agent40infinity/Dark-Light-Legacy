using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System;
using UnityScript.Steps;

[System.Serializable]
public class SettingData : MonoBehaviour
{
    public string output;

    public string keybind; //String that is used to store the converted dictionary.
    public float masterMixer; //Variable for master volume.
    public float effectsMixer; //Variable for effects volume.
    public float musicMixer; //Variable for music volume.
    public float ambienceMixer;//Variable for ambience volume.

    public SettingData(SaveState state, string[] input) //Used to set up the default data required for the json conversion.
    {
        switch (state)
        {
            case SaveState.Save:
                keybind = JsonConvert.SerializeObject(GameManager.keybind, Formatting.Indented); //Using Json converter to serialize the data within the GameManager.keybind dictionary and converts it to a string.

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
                output = keybind + "\n|\n" + JsonConvert.SerializeObject(masterMixer) + "\n|\n" + JsonConvert.SerializeObject(effectsMixer) + "\n|\n" + JsonConvert.SerializeObject(musicMixer) + "\n|\n" + JsonConvert.SerializeObject(ambienceMixer);
                break;
            case SaveState.Load:
                GameManager.keybind = JsonConvert.DeserializeObject<Dictionary<string, KeyCode>>(input[0]);
                GameManager.masterMixer.SetFloat("Master", float.Parse(input[1]));
                GameManager.masterMixer.SetFloat("Music", float.Parse(input[2]));
                GameManager.masterMixer.SetFloat("Effects", float.Parse(input[3]));
                GameManager.masterMixer.SetFloat("Ambience", float.Parse(input[4]));
                break;
        } 
    }
}
