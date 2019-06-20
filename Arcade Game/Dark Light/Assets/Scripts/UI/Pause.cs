using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*---------------------------------/
 * Script by Aiden Nathan.
 *---------------------------------*/

namespace PauseMenu
{
    public class Pause : MonoBehaviour
    {
        #region Variables
        public static bool isPaused = false; //Checks whether or not the game is paused
        public GameObject pauseMenu, options, main, mainBackground; //Creates reference for the pause menu
        public bool optionsTimer = false; //Checks whether or not the options button has been pressed
        public int oTimer = 0; //Timer for transition - options
        public bool menuTimer = false; //Checks whether or not the menu button has been pressed
        public int mTimer = 0; //Timer for transition - menu
        #endregion

        #region General
        public void Update() //Ensures the pause menu can function
        {
            if (Input.GetKeyDown(KeyCode.Escape)) //Show pause menu
            {
                if (isPaused == true)
                {
                    ResumeG();
                }
                else
                {
                    PauseG();
                }
            }

            if (optionsTimer == true) //quit Game Transition
            {
                oTimer++;
                if (oTimer >= 120)
                {
                    oTimer = 0;
                    options.SetActive(true);
                    optionsTimer = false;
                }
            }
            if (menuTimer == true) //Menu Transition
            {
                mTimer++;
                if (mTimer >= 120)
                {
                    mTimer = 0;
                    Time.timeScale = 1f;
                    main.SetActive(true);
                    mainBackground.SetActive(true);
                    menuTimer = false;
                }
            }
        }
        #endregion

        #region Pause
        public void ResumeG() //Trigger for resuming game and resume button
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }

        public void PauseG() //Trigger for pausing game
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }

        public void Menu() //Trigger for menu button
        {
            menuTimer = true;
        }

        public void Options() //Trigger for Options button
        {
            optionsTimer = true;
        }
        #endregion
    }
}


