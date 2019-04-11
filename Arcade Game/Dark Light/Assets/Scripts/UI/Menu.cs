using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace MainMenu
{
    public class Menu : MonoBehaviour
    {
        #region Variables
        //General:
        public GameObject menu, options; //Allows for reference to GameObjects Meny and Options
        public bool toggle = false; //Toggle for switching between settings and main
        public bool exitTimer = false; //Check whether or not the exit button has been pressed
        public int eTimer = 0; //Timer for transition - exit
        public bool loadTimer = false; //Checks whether or not the play button has been pressed
        public int lTimer = 0; //Timer for transition - load game
        private int sceneID; //Creates personal reference to the sceneID 

        //Settings:
        public AudioMixer mainMixer; //Creates reference for the menu music
        Resolution[] resolutions; //Creates reference for all resolutions within Unity
        public Dropdown resolutionDropdown; //Creates reference for the resolution dropdown 
        #endregion

        #region General
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

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void Update()
        {
            //Debug.Log("Load: " + loadTimer + " - Exit: " + exitTimer);
            if (Input.GetKey(KeyCode.M) == true) //Failsafe
            {
                toggle = false;
            }
            if (exitTimer == true) //Exit Transition
            {
                eTimer++;
                if (eTimer >= 120)
                {
                    eTimer = 0;
                    Application.Quit();
                    //UnityEditor.EditorApplication.isPlaying = false;
                    exitTimer = false;
                }
            }
            if (loadTimer == true) //Play Transition
            {
                lTimer++;
                if (lTimer >= 120)
                {
                    lTimer = 0;
                    SceneManager.LoadScene(sceneID);
                    loadTimer = false;
                }
            }
        }
        #endregion

        #region Main
        public void LoadScene() //Trigger for Play Button
        {
            loadTimer = true;
            sceneID = 1;
        }

        public void Exit() //Trigger for Exit Button
        {
            exitTimer = true;

        }

        public void ToggleOptions(bool toggle) //Trigger for Settings - sets active layer/pannel
        {
            if (toggle == true)
            {
                menu.SetActive(false);
                options.SetActive(true);
            }
            else if (toggle == false)
            {
                menu.SetActive(true);
                options.SetActive(false);
            }
        }
        #endregion

        #region Settings
        public void ChangeVolume(float volume) //Trigger for changing volume of game's music
        {
            mainMixer.SetFloat("volume", volume);
        }

        public void ChangeQuality(int qualityIndex) //Trigger for applying level of quality - detailing of objects
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void ToggleFullscreen(bool isFullscreen) //Trigger for applying fullscreen
        {
            Screen.fullScreen = isFullscreen;
        }

        public void ChangeResolution(int resIndex) //Trigger for changing and applying resolution based on list
        {
            Resolution resolution = resolutions[resIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
        #endregion
    }
}

