using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/*---------------------------------/
 * Script by Aiden Nathan.
 *---------------------------------*/

public class FallCheckpoint : MonoBehaviour
{
    public GameObject checkpoints;
    public static int lastPassed;
    public static Transform[] cPos;

    public void Start()
    {
        checkpoints = GameObject.Find("Checkpoints");
        cPos = GetComponentsInChildren<Transform>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var point in cPos)
        {
            Gizmos.DrawWireSphere(point.position, 1.0f);
        }
    }
}
