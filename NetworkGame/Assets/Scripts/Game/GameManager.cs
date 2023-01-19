using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    GameObject Player1Prefab;
    GameObject Player2Prefab;

    public GameObject localPlayer;
    public List<GameObject> playerList = new List<GameObject>();

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

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

    //Adds a new player to the player list and instantiates a paddle for player
    public void onPlayerConnect(int randomID)
    {
        //add player to list
        playerList.add();
        //check if list is odd or even
        //instantiate team 1 or 2 prefab paddle
    }

    //For testing currently
    public void initDefaultGameState()
    {
        playerList.Add(localPlayer);
        InstantiateTeam1Player(0);
    }

    //Instantiates a prefab for a specific playerid on the list
    void InstantiateTeam1Player(int playerID)
    {
        Player1Prefab = Resources.Load("Player1Prefab") as GameObject;
        playerList[playerID] = Instantiate(Player1Prefab);
    }

    //Instantiates a prefab for a specific playerid on the list
    void InstantiateTeam2Player(int playerID)
    {
        Player2Prefab = Resources.Load("Player2Prefab") as GameObject;
        playerList[playerID] = Instantiate(Player2Prefab);
    }

    [Header("Ball")]
    public GameObject ball;

    [Header("Player 1")]
    public GameObject Team1Goal;

    [Header("Player 2")]
    public GameObject Team2Goal;

    [Header("Score UI")]
    public GameObject Team1Text;
    public GameObject Team2Text;

    private int Team1Score;
    private int Team2Score;

    public void Team1Scored(){
        Team1Score++;
        Team1Text.GetComponent<TextMeshProUGUI>().text = Team1Score.ToString();
        ResetPosition();
    }

    public void Team2Scored(){
        Team2Score++;
        Team2Text.GetComponent<TextMeshProUGUI>().text = Team2Score.ToString();
        ResetPosition();
    }

    private void ResetPosition(){
        ball.GetComponent<Ball>().Reset();
        if (Player1Prefab != null) {
            Player1Prefab.GetComponent<Paddle>().Reset();
        }
        if (Player2Prefab != null)
        {
            Player2Prefab.GetComponent<Paddle>().Reset();
        }
    }



}
