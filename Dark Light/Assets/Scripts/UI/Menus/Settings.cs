using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public void Start()
    {
        up.text = GameManager.keybind["Up"].ToString();
        down.text = GameManager.keybind["Down"].ToString();
        left.text = GameManager.keybind["Left"].ToString();
        right.text = GameManager.keybind["Right"].ToString();
        jump.text = GameManager.keybind["Jump"].ToString();
        attack.text = GameManager.keybind["Attack"].ToString();
        dash.text = GameManager.keybind["Dash"].ToString();

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
}
