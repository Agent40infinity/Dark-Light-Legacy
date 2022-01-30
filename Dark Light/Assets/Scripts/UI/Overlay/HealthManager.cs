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
        public float imagesPerHeart; //Defines how many Images there are per heart slot.
        public bool checkHealth = true;

        public Animator[] anim = new Animator[5];
        #endregion

        #region General
        public void Start()
        {
            //anim = GetComponentsInChildren<Animator>();

            if (checkHealth == true)
            {
                CheckForHealth();
            }
        }

        public void Update() //Changes the image depending on how much health the player has.
        {
            for (int i = 0; i < anim.Length; i++)
            {
                if (Player.curHealth >= (imagesPerHeart * 2) * (i + 1)) 
                {
                }
                else
                {
                    anim[i].ResetTrigger("Recover");
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
            imagesPerHeart = (float)Player.maxHealth / (float)(anim.Length * 2.5f); //0.5
            checkHealth = false;

        }
        #endregion
    }
}