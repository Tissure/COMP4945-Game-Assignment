using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public bool isLocal;
    public float speed;
    public Rigidbody2D rb;
    public Vector3 startPos;

    // Player ID
    public int id = 111;

    private float movement;

    public Paddle(int id)
    {
        this.id = id;
        startPos = transform.position;
    }

    public Paddle(int id, float coordX, float coordY)
    {
        this.id = id;
        rb.position = new Vector2(coordX, coordY);
    }

    void Start()
    {
        startPos = transform.position;
    }

    public void SetLocal(bool local)
    {
        isLocal = local;
    }

    public bool GetLocal()
    {
        return isLocal;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocal)
        {
            movement = Input.GetAxisRaw("Vertical");
        }
        else
        {
            movement = Input.GetAxisRaw("Vertical2");
        }

        rb.velocity = new Vector2(rb.velocity.x, movement * speed);
    }

    public void Reset()
    {
        rb.velocity = Vector2.zero;
        transform.position = startPos;
    }

    public int GetID()
    {
        return id;
    }
}

