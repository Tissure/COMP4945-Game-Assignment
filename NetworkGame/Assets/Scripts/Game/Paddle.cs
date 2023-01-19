using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public bool isLocal;
    public float speed;
    public Rigidbody2D rb;
    public Vector3 startPos;

    private int ID;

    private float movement;

    void Start()
    {
        startPos = transform.position;
    }

    public void SetLocal(bool local)
    {
        isLocal = local;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocal)
        {
            movement = Input.GetAxisRaw("Vertical");
        }
        //else
        //{
        //    movement = Input.GetAxisRaw("Vertical2");
        //}

        rb.velocity = new Vector2(rb.velocity.x, movement * speed);
    }

    public void Reset(){
        rb.velocity = Vector2.zero;
        transform.position = startPos;
    }
}
