using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class DictionaryData : MonoBehaviour
{
    public string keybind; //String that is used to store the converted dictionary.

    public DictionaryData() //Used to set up the dictionary data required for the json conversion.
    {
        keybind = JsonConvert.SerializeObject(GameManager.keybind, Formatting.Indented); //Using Json converter to serialize the data within the GameManager.keybind dictionary and converts it to a string.
    }
}
