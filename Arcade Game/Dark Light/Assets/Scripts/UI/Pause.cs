using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace PauseMenu
{
    public class Pause : MonoBehaviour
    {
        #region Variables
        public static bool isPaused = false; //Checks whether or not the game is paused
        public GameObject PauseMenuUI; //Creates reference for the pause menu
        public bool quitTimer = false; //Checks whether or not the quit button has been pressed
        public int qTimer = 0; //Timer for transition - quit
        public bool menuTimer = false; //Checks whether or not the menu button has been pressed
        public int mTimer = 0; //Timer for transition - menu
        private int sceneID; //Creates personal reference to sceneID
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

            if (quitTimer == true) //quit Game Transition
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
            if (menuTimer == true) //Menu Transition
            {
                mTimer++;
                if (mTimer >= 120)
                {
                    mTimer = 0;
                    Time.timeScale = 1f;
                    SceneManager.LoadScene(sceneID);
                    menuTimer = false;
                }
            }
        }
        #endregion

        #region Pause
        public void ResumeG() //Trigger for resuming game and resume button
        {
            PauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }

        public void PauseG() //Trigger for pausing game
        {
            PauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }

        public void Menu() //Trigger for menu button
        {
            menuTimer = true;
            sceneID = 0;
        }

        public void Quit() //Trigger for quit button
        {
            quitTimer = true;
        }
        #endregion
    }
}

