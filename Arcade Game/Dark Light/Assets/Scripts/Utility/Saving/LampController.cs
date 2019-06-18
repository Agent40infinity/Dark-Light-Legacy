using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------/
 * Script by Aiden Nathan.
 *---------------------------------*/

public class LampController : MonoBehaviour
{
    public Animator Lamp;

    public void Start()
    {
        GameObject Lamp = GameObject.FindGameObjectWithTag("Save");
        Lamp.GetComponent<Animator>();
    }

    public void LightLamp()
    {
        Lamp.SetTrigger("FirstLight");
    }
}
