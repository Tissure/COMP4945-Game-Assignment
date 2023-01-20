using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

using NetworkModule;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    GameObject PlayerPrefab;
    GameObject RemotePlayerPrefab;

    public GameObject localPlayer;
    public GameObject remotePlayer;
    public List<GameObject> playerList = new List<GameObject>();

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

    //Adds a new player to the player list and instantiates a paddle for player
    public void onPlayerConnect(params int[] numbers)
    {

        //check if list is odd or even
        //instantiate team 1 or 2 prefab paddle
        //add player to list
        //playerList.Add();
    }

    //For testing currently
    public void initDefaultGameState()
    {
        playerList.Add(localPlayer);
        InstantiatePlayer(0, 1);
        //check if player list is more than 1
        //if not pause game
        playerList.Add(remotePlayer);
        InstantiatePlayer(1, 2);

    }

    //Instantiates a prefab for a specific playerid on the list
    void InstantiatePlayer(int playerID, int teamNum)
    {
        PlayerPrefab = Resources.Load("PlayerPrefab") as GameObject;
        GameObject player = Instantiate(PlayerPrefab);
        if (teamNum == 1)
        {
            player.transform.position = new Vector2(-8, 0);
            player.GetComponent<Paddle>().SetLocal(true);
        } else
        {
            player.transform.position = new Vector2(8, 0);
            player.GetComponent<Paddle>().SetLocal(false);
        }
        playerList[playerID] = player;
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
        PlayerPrefab.GetComponent<Paddle>().Reset();
        
    }

    public string generateUniqueID()
    {
        // Testing
        return (playerList.Count + 1).ToString();
    }

    public void Update()
    {
        // MonoBehaviour Update() is called every frame.
        PacketHandler packet = new PacketHandler();
        GameManager.getInstance.playerList.Add(localPlayer);
        string payload = packet.buildPacket("Player-Connection");
        Multicast multicast = new Multicast();
        multicast.Send(payload);
    }

}
