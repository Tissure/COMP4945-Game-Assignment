using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using NetworkModule;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager getInstance { get { return _instance; } }
    private void Awake()
    {
        if (_instance !=null && _instance != this) 
        {
            Destroy(this);
        } 
        else
        {
            _instance = this;
        }
    }

    // Game State
    public Paddle localPlayer;
    public List<Paddle> playerList = new List<Paddle>();

    [Header("Ball")]
    public GameObject ball;

    [Header("Player 1")]
    public GameObject player1Paddle;
    public GameObject player1Goal;

    [Header("Player 2")]
    public GameObject player2Paddle;
    public GameObject player2Goal;

    [Header("Score UI")]
    public GameObject Player1Text;
    public GameObject Player2Text;

    private int Player1Score;
    private int Player2Score;

    public void Player1Scored(){
        Player1Score++;
        Player1Text.GetComponent<TextMeshProUGUI>().text = Player1Score.ToString();
        ResetPosition();
    }

    public void Player2Scored(){
        Player2Score++;
        Player2Text.GetComponent<TextMeshProUGUI>().text = Player2Score.ToString();
        ResetPosition();
    }

    private void ResetPosition(){
        ball.GetComponent<Ball>().Reset();
        player1Paddle.GetComponent<Paddle>().Reset();
        player2Paddle.GetComponent<Paddle>().Reset();
    }

    public void Update()
    {
        PacketHandler packet = new PacketHandler();
        string payload = packet.buildPacket("Player");
        Multicast multicast = new Multicast();
        multicast.Send(payload);
    }

}
