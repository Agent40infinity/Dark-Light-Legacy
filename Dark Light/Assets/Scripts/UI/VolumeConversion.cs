using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeConversion : MonoBehaviour
{
    public Slider slider;
    public string name;

    public void Awake()
    {
        name = gameObject.name;
        slider = gameObject.GetComponent<Slider>();
        float value;
        if (GameManager.masterMixer.GetFloat(name, out value)) //If the mixer within MasterMixer exists, saves the value to it's respective variable so json conversion - Repeats 4 times for each Mixer.
        { 
            slider.value = Mathf.Pow(10, (value / 20));
        }
    }
}
