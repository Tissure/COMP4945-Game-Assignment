using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Assets.Scripts.Game;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public AudioHandler audioHandler;
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

    private void OnCollisionEnter2D(Collision2D collision) {
        string other = collision.gameObject.name;
        if ( other == null) {
            return;
        }
       
        if (other.ToString().Contains("Player")){
            Debug.Log(other.ToString());
            audioHandler.setBallHitPlayer();
        }
        if (other.ToString().Contains("Wall")) {
            Debug.Log(other.ToString());
            audioHandler.setBallHitWall();
        }
        audioHandler.playOnce();
    }
    
}
