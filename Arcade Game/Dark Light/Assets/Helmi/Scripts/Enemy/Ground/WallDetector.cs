using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    #endregion
}
