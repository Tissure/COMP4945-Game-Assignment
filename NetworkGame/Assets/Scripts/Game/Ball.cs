using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    public Vector3 startpos;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;
        Launch();   
    }

    public void Reset()
    {
        rb.velocity = Vector2.zero;
        rb.position = startpos;
        Launch();        
    }

    private void Launch(){
        // Launch the ball
        //float x = Random.Range(0, 2) == 0 ? -1 : 1;
        //float y = Random.Range(0, 2) == 0 ? -1 : 1;
        rb.velocity = new Vector2(1 * speed, 1 * speed);
    }
}
