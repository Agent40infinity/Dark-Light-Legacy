using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

/*---------------------------------/
 * Script by Aiden Nathan.
 * Set up Assistance by Helmi.
 *---------------------------------*/

public class Menu : MonoBehaviour
{
    #region Variables
    //General: 
    public GameObject main, mainBackground, fade, options, general, video, audio, controls, overlay, saveload; //Allows for reference to GameObjects Meny and Options
    public AudioMixer masterMixer;                                                                             //public bool toggle = false; //Toggle for switching between settings and main
                                                                                                               //public int option = 0; //Changes between the 4 main screens in options.
    public bool quitTimer = false; //Check whether or not the exit button has been pressed
    public int qTimer = 0; //Timer for transition - exit
    public bool startTimer = false; //Checks whether or not the play button has been pressed
    public int sTimer = 0; //Timer for transition - load game

    //Settings:
    Resolution[] resolutions; //Creates reference   for all resolutions within Unity
    public Dropdown resolutionDropdown; //Creates reference for the resolution dropdown 

    //Controls:
    public Text up, down, left, right, jump, attack, dash;
    private GameObject currentKey;

    //Music:
    public AudioSource music;

    public LastMenuState lastMenuState;
    public GameObject pauseMenu;

    public Settings optionsMenu;
    #endregion

    public void Start() //Used to load resolutions and create list for the dropdown, collects both Width and Height seperately
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

        music = GameObject.FindGameObjectWithTag("MenuMusic").GetComponent<AudioSource>();
        music.Play();

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

    public void Update()
    {

        if (quitTimer == true) //Exit Transition
        {
            qTimer++;
            if (qTimer >= 120)
            {
                qTimer = 0;
                Application.Quit();
                //UnityEditor.EditorApplication.isPlaying = false;
                quitTimer = false;
            }
        }
        if (startTimer == true) //Play Transition
        {
            sTimer++;
            if (sTimer >= 120)
            {
                sTimer = 0;
                saveload.SetActive(false);
                mainBackground.SetActive(false);
                overlay.SetActive(true);
                startTimer = false;
                Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
                SystemSave.LoadPlayer(player, GameManager.loadedSave);

                fade.GetComponent<FadeController>().FadeIn();
                GameManager.gameActive = true;
            }
        }
    }

    public void SaveSelection(bool toggle)
    {
        if (toggle == true)
        {
            main.SetActive(false);
            saveload.SetActive(true);
        }
        else
        {
            main.SetActive(true);
            saveload.SetActive(false);
        }

    }

    public void StartGame() //Trigger for Play Button
    {
        startTimer = true;
        music.Stop();
        fade.GetComponent<FadeController>().FadeOut();
    }

    public void Quit() //Trigger for Exit Button
    {
        quitTimer = true;

    }

    public void OptionsCall(bool toggle)
    {
        optionsMenu.ToggleOptions(toggle, LastMenuState.MainMenu);
    }
}