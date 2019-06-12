using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float speed = 20f;
    private Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Fire(Vector3 direction)
    {
        rigid.AddForce(direction * speed * Time.deltaTime, ForceMode2D.Impulse);
    }
}
