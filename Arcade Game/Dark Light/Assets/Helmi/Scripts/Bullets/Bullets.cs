using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float speed = 20f;
    private Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Fire(Vector2 direction)
    {
        rigid.AddForce(direction * speed * Time.deltaTime, ForceMode2D.Impulse);
    }
}
