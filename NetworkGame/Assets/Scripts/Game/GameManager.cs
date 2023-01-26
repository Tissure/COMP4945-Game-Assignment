using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using NetworkModule;
using Unity.VisualScripting;
using System;

public class GameManager : MonoBehaviour
{
    Multicast multicast;
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
            multicast = GameObject.Find("Preloader").GetComponent<Multicast>();
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
        // Gets LocalIP and uses the last number of ip as ID   eg. 192.168.1.ID  
        string uniqueID = GameObject.Find("Preloader").GetComponent<Preloader>().multicast.GetIP();
              
        // If playerList is even assign to team1, if odd assign team2
        if (playerList.Count % 2 == 0)
        {
            localPlayer = InstantiatePlayer(uniqueID, 1);
        } else
        {
            localPlayer = InstantiatePlayer(uniqueID, 2);
        }
        
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
                player.transform.position = new Vector2(-8, 0);
                //player.GetComponent<Paddle>().SetLocal(true);
                player.GetComponent<Paddle>().SetID(playerID);
                player.GetComponent<Paddle>().SetTeam(teamNum);
            }
            else
            {
                player.transform.position = new Vector2(8, 0);
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
    }

    public void Team2Scored()
    {
        Team2Score++;
        Team2Text.GetComponent<TextMeshProUGUI>().text = Team2Score.ToString();
        ResetPosition();
    }

    private void ResetPosition()
    {
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
        
        //GameManager.getInstance.playerList.Add(localPlayer);
        string payload = packet.buildPacket("Player-Connection");
        
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


}




/*using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using NetworkModule;
using Unity.VisualScripting;
using System.Threading;

public class GameManager : MonoBehaviour
{
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
    public void InstantiatePlayer(int playerID, int teamNum)
    {
        PlayerPrefab = Resources.Load("PlayerPrefab") as GameObject;
        GameObject player = Instantiate(PlayerPrefab);
        if (teamNum == 1)
        {
            player.transform.position = new Vector2(-8, 0);
            player.GetComponent<Paddle>().SetLocal(true);
        }
        else
        {
            player.transform.position = new Vector2(8, 0);
            player.GetComponent<Paddle>().SetLocal(false);
        }
        playerList[playerID] = player;
    }

    public void Team1Scored()
    {
        Team1Score++;
        // Team1Text.GetComponent<TextMeshProUGUI>().text = Team1Score.ToString();
        ResetPosition();
    }

    public void Team2Scored()
    {
        Team2Score++;
        //Team2Text.GetComponent<TextMeshProUGUI>().text = Team2Score.ToString();
        ResetPosition();
    }

    private void ResetPosition()
    {
        ball.GetComponent<Ball>().Reset();
        PlayerPrefab.GetComponent<Paddle>().Reset();

    }

    public int generateUniqueID()
    {
        // Testing
        return (playerList.Count + 1);
    }

 *//*   public int getLocalPlayerID()
    {
        this.Invoke(() =>
        {
            return localPlayer.GetComponentFastPath<Paddle>().GetID();
            // do something with the component
        });
        return 4242;

    }*//*

    public void Update()
    {
        // MonoBehaviour Update() is called every frame.
        PacketHandler packet = new PacketHandler();
        //GameManager.getInstance.playerList.Add(localPlayer);
        string payload = packet.buildPacket("Ball");
        Multicast multicast = new Multicast();

        UnityEngine.Debug.Log("Current Thread ID:IN UPDDATE " + Thread.CurrentThread.ManagedThreadId);
        UnityEngine.Debug.Log("Current Thread Name:IN UPDATE " + Thread.CurrentThread.Name);
        UnityEngine.Debug.Log("Current Thread Priority: IN UPDATE" + Thread.CurrentThread.Priority);
        multicast.Send(payload);

    }

    public void AddNewPlayer(int playerID)
    {
        GameObject player = Instantiate(PlayerPrefab);
        playerList.Add(player);
    }

    public void SetLocalPlayer(GameObject player)
    {
        localPlayer = player;
    }

    public void SetBall(float coordX, float coordY)
    {
        ball.transform.position = new Vector2(coordX, coordY);
        UnityEngine.Debug.Log("hjkeshafkjldshfsq");
    }
}
*/
/*
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager instance;

    // Enum to define the different teams
    public enum Team
    {
        TEAM_A,
        TEAM_B
    }

    // Dictionary to store player information
    public Dictionary<string, Player> players = new Dictionary<string, Player>();

    // Game state variables
    public Vector2 ballPosition;

    // Score variables
    public int teamAScore;
    public int teamBScore;

    // Game state
    public enum GameState
    {
        WAITING,
        PLAYING,
        PAUSED,
        FINISHED
    }
    public GameState gameState;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ConnectPlayer(string playerId, Vector2 playerPosition)
    {
        Player newPlayer = new Player(playerId, playerPosition, Team.TEAM_A);
        players.Add(playerId, newPlayer);

        // If we have enough players, start the game
        if (players.Count >= 2)
        {
            gameState = GameState.PLAYING;
        }
    }

    public void DisconnectPlayer(string playerId)
    {
        players.Remove(playerId);

        // If we have less than two players, stop the game
        if (players.Count < 2)
        {
            gameState = GameState.WAITING;
        }
    }

    public void UpdatePlayerPosition(string playerId, Vector2 playerPosition)
    {
        players[playerId].position = playerPosition;
    }

    public void UpdateBallPosition(Vector2 ballPosition)
    {
        this.ballPosition = ballPosition;
    }

    public void AddScore(Team team)
    {
        if (team == Team.TEAM_A)
        {
            teamAScore++;
        }
        else if (team == Team.TEAM_B)
        {
            teamBScore++;
        }
        if (teamAScore >= 10 || teamBScore >= 10)
        {
            gameState = GameState.FINISHED;
            // Show the winner and reset the scores
            string winner = (teamAScore > teamBScore) ? "Team A" : "Team B";
            Debug.Log("Winner: " + winner);
            teamAScore = 0;
            teamBScore = 0;
        }
    }

    public void PauseGame()
    {
        gameState = GameState.PAUSED;
    }

    public void ResumeGame()
    {
        gameState = GameState.PLAYING;
    }

    // Player class to store player information
    public class Player
    {
        public string id;
        public Vector2 position;
        public Team team;

        public Player(string id, Vector2 position, Team team)
        {
            this.id = id;
            this.position = position;
            this.team = team;
        }
    }
}*/