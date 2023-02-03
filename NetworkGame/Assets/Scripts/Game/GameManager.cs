using System.Collections.Generic;
using TMPro;
using UnityEngine;
using NetworkModule;
using System;

public class GameManager : MonoBehaviour
{
    WebSocketNetwork multicast;
    PacketHandler packet;

    GameObject PlayerPrefab;
    GameObject RemotePlayerPrefab;

    public GameObject localPlayer;
    public GameObject remotePlayer;
    public List<GameObject> playerList = new List<GameObject>();
    
    [Header("Ball")]
    public GameObject ball;

    [Header("Goal Hit Boxes")]
    public GameObject Team1Goal;
    public GameObject Team2Goal;

    [Header("Score UI")]
    public GameObject Team1Text;
    public GameObject Team2Text;

    private int Team1Score;
    private int Team2Score;

    private string uniqueID;

    private static GameManager _instance;
    public static GameManager getInstance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
            multicast = GameObject.Find("Preloader").GetComponent<WebSocketNetwork>();
            packet = new PacketHandler();
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
        Awake();
        
        // Gets LocalIP and uses the last number of ip as ID   eg. 192.168.1.ID  
        //uniqueID = multicast.GetIP();
        uniqueID = generateUniqueID();
        multicast.Send(packet.buildPacket("Player-Connection"));
        //string uniqueID = "192.168.1.111";
        // If playerList is even assign to team1, if odd assign team2
        if (playerList.Count % 2 == 0)
        {
            localPlayer = InstantiatePlayer(uniqueID, 2);
        } else
        {
            localPlayer = InstantiatePlayer(uniqueID, 1);
        }
        localPlayer.GetComponent<Paddle>().SetLocal(true);
        
    }

    //Instantiates a prefab for a specific playerid on the list
    public GameObject InstantiatePlayer(string playerID, int teamNum)
    {
        bool newPlayer = true;
        GameObject player = null;
        foreach (GameObject tempPlayer in playerList)
        {
            if (tempPlayer.GetComponent<Paddle>().GetID() == playerID)
            {
                Debug.Log("PLAYER EXISTS");
                newPlayer = false;
                player = tempPlayer;
                break;
            }
        }

        if (newPlayer)
        {
            PlayerPrefab = Resources.Load("PlayerPrefab") as GameObject;
             player = Instantiate(PlayerPrefab);
            if (teamNum == 1)
            {
                player.GetComponent<Paddle>().rb.position = new Vector2(-8, 0);
                //player.GetComponent<Paddle>().SetLocal(true);
                player.GetComponent<Paddle>().SetID(playerID);
                player.GetComponent<Paddle>().SetTeam(teamNum);
            }
            else
            {
                player.GetComponent<Paddle>().rb.position = new Vector2(8, 0);
                //player.GetComponent<Paddle>().SetLocal(false);
                player.GetComponent<Paddle>().SetID(playerID);
                player.GetComponent<Paddle>().SetTeam(teamNum);
            }
        
            playerList.Add(player);
            return player;
        }

        return player;
       
    }

    public void Team1Scored()
    {
        Team1Score++;
        Team1Text.GetComponent<TextMeshProUGUI>().text = Team1Score.ToString();
        ResetPosition();
        string payload = packet.buildPacket("Ball");

        multicast.Send(payload);
    }

    public void Team2Scored()
    {
        Team2Score++;
        Team2Text.GetComponent<TextMeshProUGUI>().text = Team2Score.ToString();
        ResetPosition();
        string payload = packet.buildPacket("Ball");

        multicast.Send(payload);
    }

    private void ResetPosition()
    {
        ball.GetComponent<Ball>().Reset();
        PlayerPrefab.GetComponent<Paddle>().Reset();
    
    }

    public string generateUniqueID()
    {
        // Testing
        return Guid.NewGuid().ToString();
    }

    public void Update()
    {
        // MonoBehaviour Update() is called every frame.

        //GameManager.getInstance.playerList.Add(localPlayer);
        string payload = packet.buildPacket("Player");
        //Debug.Log(payload);
        multicast.Send(payload);



    }

    public void SendGameState()
    {

        multicast.Send(packet.buildPacket("Update-GameState"));
    }

    public int GetTeam1Score()
    {
        return Team1Score;
    }

    public void SetTeam1Score(int score)
    {
        Team1Score = score;
    }

    public int GetTeam2Score()
    {
        return Team2Score;
    }

    public void SetTeam2Score(int score)
    {
        Team2Score = score;
    }

    public void SetBall(float x, float y, Vector2 ballSpeed)
    {
        ball.GetComponent<Ball>().rb.position = new Vector2(x, y);
        ball.GetComponent<Ball>().rb.velocity = ballSpeed;
    }

    public void DisconnectPlayer(string playerID)
    {
        Debug.Log("trying to Disconnecting playerid" + playerID);
        foreach (var player in playerList)
        {
            if (player.GetComponent<Paddle>().GetID() == playerID)
            {
                Debug.Log("Disconnecting playerid" + playerID);
                playerList.Remove(player);
            }
            
        }
        
    }
    
    public void UpdatePlayerPosition(string playerID, float coordX, float coordY) 
    {
        foreach (var player in playerList)
        {
            if (player.GetComponent<Paddle>().GetID() == playerID)
            {
                //Debug.Log("FREEEEZEEE");
                player.GetComponent<Paddle>().rb.position = new Vector2(coordX, coordY);
           
           
            }

        }
    }

   public string GetUniqueID()
    {
        return uniqueID;
    }

}
