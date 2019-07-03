using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==========================================
// Title:  Dark Light
// Author: Helmi Amsani
// Date:   14 May 2019
//==========================================

public class WallDetector : MonoBehaviour
{
    #region Variables
    public Enemy enemy;
    #endregion

    #region When This Collider Hit any collider but Player
    public void OnCollisionEnter2D(Collision2D collision)
    {    
        if (collision.gameObject.tag != "Player")
        {
            BaseMovement();
        }
    }
    #endregion

    public void BaseMovement()
    {
        // When enemy moves right
        if (enemy.moveRight == true)
        {
            enemy.SetDirection(new Vector3(0, -180, 0), false);
        }
        // When enemy moves left
        else
        {
            enemy.SetDirection(Vector3.zero, true);
        }
    }
}
