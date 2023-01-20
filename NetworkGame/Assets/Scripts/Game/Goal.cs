using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public bool isTeam1Goal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (!isTeam1Goal)
            {
                Debug.Log("Player 2 scored..!");
                GameObject.Find("GameManager").GetComponent<GameManager>().Team1Scored();
            }
            else
            {
                Debug.Log("Player 1 scored..!");
                GameObject.Find("GameManager").GetComponent<GameManager>().Team2Scored();
            }
        }
    }
}
