using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
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

    //public UnityEvent OnPlayerInstantiated;

    //void Start()
    //{

    //    OnPlayerInstantiated.AddListener(InstantiateTeam1Player(2));
    //}

    //public void onPlayerConnect()
    //{
    //    //add player to list
    //    playerList.add();
    //}

    GameObject Player1Prefab;
    GameObject Player2Prefab;

    public GameObject localPlayer;
    public List<GameObject> playerList = new List<GameObject>();

    public void initDefaultGameState()
    {
        playerList.Add(localPlayer);
        //OnPlayerInstantiated.AddListener(InstantiateTeam1Player(0));
        InstantiateTeam1Player(0);
    }

    void InstantiateTeam1Player(int playerID)
    {
        Player1Prefab = Resources.Load("Player1Prefab") as GameObject;
        playerList[playerID] = Instantiate(Player1Prefab);
    }

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
