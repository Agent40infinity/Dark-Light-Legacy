using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    public Animator Fade; //Creates a reference for the Animator: Fade.

    public void FadeOut() //Called upon to Fade Out.
    {
        Fade.SetTrigger("FadeOut");
    }

    public void FadeIn() //Called upon to Fade In.
    {
        Fade.SetTrigger("FadeIn");
    }
}
