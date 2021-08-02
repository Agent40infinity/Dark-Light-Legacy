using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject mainMenu, options, general, video, audio, controls;
    public AudioMixer masterMixer;

    Resolution[] resolutions; //Creates reference for all resolutions within Unity
    public Dropdown resolutionDropdown; //Creates reference for the resolution dropdown 

    public Text up, down, left, right, jump, attack, dash;
    private GameObject currentKey;

    public LastMenuState lastMenuState;
    public GameObject pauseMenu;

    public void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++) //Load possible resolutions into list
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) //Makes sure the resolution is correctly applied
            {
                currentResolutionIndex = i;
            }
        }

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

    public void OptionsCall(bool toggle)
    {
        ToggleOptions(toggle, LastMenuState.MainMenu);
    }

    public void ToggleOptions(bool toggle, LastMenuState lastState) //Trigger for Settings - sets active layer/pannel
    {
        if (toggle == true)
        {
            lastMenuState = lastState;
            mainMenu.SetActive(false);
            pauseMenu.SetActive(false);
            options.SetActive(true);
        }
        else if (toggle == false)
        {
            switch (lastMenuState)
            {
                case LastMenuState.MainMenu:
                    mainMenu.SetActive(true);
                    break;
                case LastMenuState.PauseMenu:
                    pauseMenu.SetActive(true);
                    break;
            }
            options.SetActive(false);
        }
    }

    public void ChangeBetween(int option) //Trigger for Settings - sets active layer/pannel
    {
        switch (option)
        {
            case 0:
                general.SetActive(true);
                video.SetActive(false);
                audio.SetActive(false);
                controls.SetActive(false);

                SystemConfig.SaveSettings();
                break;
            case 1:
                general.SetActive(false);
                video.SetActive(true);
                audio.SetActive(false);
                controls.SetActive(false);
                break;
            case 2:
                general.SetActive(false);
                video.SetActive(false);
                audio.SetActive(true);
                controls.SetActive(false);
                break;
            case 3:
                general.SetActive(false);
                video.SetActive(false);
                audio.SetActive(false);
                controls.SetActive(true);
                break;
        }
    }

    public void MasterVolume(float volume) //Trigger for changing volume of game's master channel
    {
        GameManager.masterMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void EffectsVolume(float volume) //Trigger for changing volume of game's sfx channel
    {
        GameManager.masterMixer.SetFloat("Effects", Mathf.Log10(volume) * 20);
    }

    public void MusicVolume(float volume) //Trigger for changing volume of game's music channel
    {
        GameManager.masterMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void AmbienceVolume(float volume) //Trigger for changing volume of game's music channel
    {
        GameManager.masterMixer.SetFloat("Ambience", Mathf.Log10(volume) * 20);
    }

    public void ChangeQuality(int qualityIndex) //Trigger for applying level of quality - detailing of objects
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void ToggleFullscreen(int option) //Trigger for applying fullscreen
    {
        switch (option)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }

    public void ChangeResolution(int resIndex) //Trigger for changing and applying resolution based on list
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void OnGUI()
    {
        if (currentKey != null) //Checks whether or not there is a Keycode saved to 'currentKey'
        {
            Event keypress = Event.current; //Creates an event called keypress
            if (keypress.isKey) //Checks whether or not the event "keypress" contains a keycode
            {
                GameManager.keybind[currentKey.name] = keypress.keyCode; //Saves the keycode from the event as the keycode attached to the keybind dictionary
                currentKey.transform.GetChild(0).GetComponent<Text>().text = keypress.keyCode.ToString(); //Changes the text to match that of the keycode replacing the previous one
                currentKey = null; //resets the currentKey putting it back to null
            }
            else if (keypress.shift)
            {
                GameManager.keybind[currentKey.name] = KeyCode.LeftShift;
                currentKey.transform.GetChild(0).GetComponent<Text>().text = GameManager.keybind[currentKey.name].ToString();
                currentKey = null;
            }
        }
    }

    public void changeControls(GameObject clicked) //Trigger for changing any one of the keybinds
    {
        currentKey = clicked;
    }
}

public enum LastMenuState
{
    MainMenu,
    PauseMenu
}