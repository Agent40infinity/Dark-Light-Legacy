using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*---------------------------------/
 * Script created by Aiden Nathan.
 *---------------------------------*/

namespace HealthManagement
{
    public class HealthManager : MonoBehaviour
    {
        #region Variables
        public Image[] heartSlots = new Image[5]; //Array used to reference the individual heart locations.
        public Sprite[] hearts = new Sprite[2]; //Array used to reference the images used per heart.
        private float imagesPerHeart; //Defines how many Images there are per heart slot.

        public Animator[] anim = new Animator[5];
        #endregion

        #region General
        public void Start()
        {
            CheckForHealth();
        }

        public void Update() //Changes the image depending on how much health the player has.
        {
            for (int i = 0; i < heartSlots.Length; i++)
            {
                if (Player.curHealth >= (imagesPerHeart * 2) * (i + 1)) 
                {
                }
                else
                {
                    Debug.Log("Still doing this");
                    anim[i].SetTrigger("Lose");
                }
            }

            if (Player.recovered == true)
            {     
                for (int i = 0; i < anim.Length; i++)
                {
                    anim[i].ResetTrigger("Lose");
                    anim[i].SetTrigger("Recover");
                }
                Player.recovered = false;
            }
        }
        #endregion
        #region CheckForHealth
        public void CheckForHealth() //Calculates the health per slot.
        {
            imagesPerHeart = (float)Player.maxHealth / (float)(heartSlots.Length * 2.5f); //0.5
        }
        #endregion
    }
}