using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
 
[System.Serializable]
public class SettingData : MonoBehaviour
{
    public string keybind;
    public float[] mixer = new float[4];

    public SettingData()
    {
        keybind = JsonConvert.SerializeObject(GameManager.keybind, Formatting.Indented);
        Debug.Log(keybind);
    }
}
