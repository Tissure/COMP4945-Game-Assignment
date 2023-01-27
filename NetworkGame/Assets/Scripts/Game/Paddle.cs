using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    private bool isLocal;
    private float speed;
    public Rigidbody2D rb;
    private Vector2 startPos;

    // Player ID
    private string id;
    private int teamNum;

    private float movement;

   
    void Start()
    {
        speed = 10f;
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

    public string GetID()
    {
        return id;
    }
    public void SetID(string ID)
    {
        this.id = ID;
    }

    public void SetTeam(int team)
    {
        this.teamNum = team;
    }

    public int GetTeam()
    {
        return teamNum;
    }

    public Rigidbody2D GetRB()
    {
        return rb;
    }

}

