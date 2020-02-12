using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------/
 * Script by Aiden Nathan.
 *---------------------------------*/

public class Lamp : MonoBehaviour
{
    #region Variables
    public GameObject lamps; //Reference for the parented GameObject.
    public static int lastSaved; //Index for the save points.
    public static Transform[] lPos; //Array of transforms used to store the positions of the children within the parented GameObject.
    public static bool[] lLight;
    #endregion


    #region General
    public void Start() //Defaults first spawn as the first save point and gathers all information required to store the values of each save point.
    {
        lastSaved = 1;
        lamps = GameObject.Find("Lamps");
        lPos = lamps.GetComponentsInChildren<Transform>();
        lLight = new bool[lamps.GetComponentsInChildren<Transform>().Length];
        Debug.Log("Lights length: " + lLight.Length);
    }
    #endregion
}
