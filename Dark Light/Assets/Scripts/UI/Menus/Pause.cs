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
        public GameObject pauseMenu;
        public GameObject options, main, mainBackground, fade, overlay; //Creates reference for the pause menu
        public bool menuTimer = false; //Checks whether or not the menu button has been pressed
        public int mTimer = 0; //Timer for transition - menu
        public Menu mainMenu;
        #endregion

        #region General
        public void Update() //Ensures the pause menu can function
        {
            if (Input.GetKeyDown(KeyCode.Escape) && GameManager.gameActive) //Show pause menu
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

            if (menuTimer == true) //Menu Transition
            {
                mTimer++;
                if (mTimer >= 120)
                {
                    mTimer = 0;
                    Time.timeScale = 1f;
                    pauseMenu.SetActive(false);
                    main.SetActive(true);
                    overlay.SetActive(false);
                    mainBackground.SetActive(true);
                    menuTimer = false;
                    Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                    SystemSave.SavePlayer(player, GameManager.loadedSave);

                    fade.GetComponent<FadeController>().FadeIn();
                    GameManager.gameActive = false;
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
            fade.GetComponent<FadeController>().FadeOut();
        }

        public void OptionsCall(bool toggle)
        {
            mainMenu.ToggleOptions(toggle, LastMenuState.PauseMenu);
        }
        #endregion
    }
}


