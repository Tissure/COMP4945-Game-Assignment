using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

namespace NetworkModule
{
    public class PacketHandler
    {
        public struct Packet
        {
            public byte[] data;

            public Packet(byte[] data)
            {
                this.data = data;
            }
        }

       
        public static class Constants
        {
            public const string BOUNDARY = "--boundary";
            public const string CRLF = "\r\n";
            public const string CONTENTTYPEFORMAT = "Content-Type:{0}";
            public const string IDFORMAT = "id:{0}";
            public const string COORDFORMAT = "{0}coord:{1}";
            public const string EOT = "\\4";

            // Player Information
            public const string CONTENTTYPEREGEX = @"\bContent-Type:.*\b";
            public const string IDREGEX = @"\bid:\d*\b";
            public const string XCOORDREGEX = @"\bXcoord:\d*\b";
            public const string YCOORDREGEX = @"\bYcoord:\d*\b";
        }

        public string buildPlayerDisconnectBodyPart()
        {
            GameManager gameState = (GameManager)GameObject.Find("GameManager").GetComponent("GameManager");
            string payload = "";
            StringBuilder stringBuilder = new StringBuilder(payload);
            stringBuilder.Append(Constants.BOUNDARY).Append(Constants.CRLF);

            // Header
            stringBuilder.AppendFormat(Constants.CONTENTTYPEFORMAT, "Player-Disconnect").Append(Constants.CRLF).Append(Constants.CRLF);

            // Payload
            stringBuilder.AppendFormat(Constants.IDFORMAT, gameState.localPlayer.GetComponent<Paddle>().GetID()).Append(Constants.CRLF);
            return stringBuilder.ToString();
        }

        public string buildPlayerConnectIDBodyPart()
        {
            GameManager gameState = (GameManager)GameObject.Find("GameManager").GetComponent("GameManager");
            string payload = "";
            StringBuilder stringBuilder = new StringBuilder(payload);
            stringBuilder.Append(Constants.BOUNDARY).Append(Constants.CRLF);

            // Header
            stringBuilder.AppendFormat(Constants.CONTENTTYPEFORMAT, "New-Player-ID").Append(Constants.CRLF).Append(Constants.CRLF);

            // Payload
            stringBuilder.AppendFormat(Constants.IDFORMAT, gameState.generateUniqueID()).Append(Constants.CRLF);
            return stringBuilder.ToString();
        }

        public string buildPlayerConnectionPacket()
        {
            GameManager gameState = GameManager.getInstance;
            string payload = "";
            StringBuilder stringBuilder = new StringBuilder(payload);
            stringBuilder.Append(Constants.BOUNDARY).Append(Constants.CRLF);

            // Header Info
            stringBuilder.AppendFormat(Constants.CONTENTTYPEFORMAT, "Player-Connection").Append(Constants.CRLF).Append(Constants.CRLF);

            stringBuilder.AppendFormat("New Player ID:"+ gameState.GetUniqueID()).Append(Constants.CRLF);
            /* stringBuilder.AppendFormat("New Player X:" + gameState.localPlayer.GetComponent<Paddle>().rb.position.x).Append(Constants.CRLF);
             stringBuilder.AppendFormat("New Player Y:" + gameState.localPlayer.GetComponent<Paddle>().rb.position.y).Append(Constants.CRLF);
             stringBuilder.AppendFormat("New Player Team:" + gameState.localPlayer.GetComponent<Paddle>().GetTeam()).Append(Constants.CRLF).Append(Constants.CRLF);*/
            // Payload - Current GameState + New Player's ID
            // New Player's ID
            //stringBuilder.Append(buildPlayerConnectIDBodyPart());
            // Current Ball Position
            //stringBuilder.Append(buildBallPositionBodyPart(gameState.ball.transform.position.x, gameState.ball.transform.position.y));
            // Current List of Players
            //stringBuilder.Append(buildPlayerListBodyPart(gameState.playerList));

            // Delimit end of message
            //stringBuilder.Append(Constants.BOUNDARY).Append(Constants.CRLF).Append(Constants.CRLF);
            //stringBuilder.Append(Constants.EOT).Append(Constants.CRLF);

            return stringBuilder.ToString();

        }

        /// <summary>
        /// Helper Function to build Multi-Part bodypart
        /// </summary>
        /// <returns>MIME Multipart-Form Bodypart containing payload as a string.</returns>
        public string buildPlayerBodyPart(string id, int team, float xCoord, float yCoord)
        {
            string payload = "";
            StringBuilder stringBuilder = new StringBuilder(payload);

            // Payload
            stringBuilder.AppendFormat(Constants.IDFORMAT, id).Append(Constants.CRLF);
            stringBuilder.AppendFormat("Player Team:" + team.ToString()).Append(Constants.CRLF);
            stringBuilder.AppendFormat(Constants.COORDFORMAT, "X", xCoord.ToString()).Append(Constants.CRLF);
            stringBuilder.AppendFormat(Constants.COORDFORMAT, "Y", yCoord.ToString()).Append(Constants.CRLF);

            // Return payload
            return stringBuilder.ToString();

        }

        public string buildPlayerListBodyPart(List<GameObject> playerList)
        {
            GameManager gameState = (GameManager)GameObject.Find("GameManager").GetComponent("GameManager");
            string payload = "";
            StringBuilder stringBuilder = new StringBuilder(payload);

            stringBuilder.Append(Constants.BOUNDARY).Append(Constants.CRLF);

            // Header Info
            stringBuilder.AppendFormat(Constants.CONTENTTYPEFORMAT, "PlayerList").Append(Constants.CRLF).Append(Constants.CRLF);

            int i = 0;
            // Payload
            foreach (GameObject player in gameState.playerList)
            {
                i++;
               // stringBuilder.Append(buildPlayerBodyPart(player.GetComponent<Paddle>().GetID(), player.GetComponent<Paddle>().rb.position.x, player.GetComponent<Paddle>().rb.position.y));
            }

            // Return payload
            return stringBuilder.ToString();

        }

        public string buildBallPositionBodyPart(float xCoord, float yCoord)
        {
            string payload = "";
            StringBuilder stringBuilder = new StringBuilder(payload);

            stringBuilder.Append(Constants.BOUNDARY).Append(Constants.CRLF);

            // Header Info
            stringBuilder.AppendFormat(Constants.CONTENTTYPEFORMAT, "Ball").Append(Constants.CRLF).Append(Constants.CRLF);

            // Start of Payload
            stringBuilder.AppendFormat(Constants.COORDFORMAT, "X", xCoord.ToString()).Append(Constants.CRLF);
            stringBuilder.AppendFormat(Constants.COORDFORMAT, "Y", yCoord.ToString()).Append(Constants.CRLF);

            return stringBuilder.ToString();
        }

        public string buildUpdateGameStatePacket()
        {
            GameManager gameState = GameManager.getInstance;
            string payload = "";
            StringBuilder stringBuilder = new StringBuilder(payload);

            stringBuilder.Append(Constants.BOUNDARY).Append(Constants.CRLF);

            stringBuilder.AppendFormat(Constants.CONTENTTYPEFORMAT, "Update-GameState").Append(Constants.CRLF).Append(Constants.CRLF);

            stringBuilder.AppendFormat("Team1 Score:" + gameState.GetTeam1Score()).Append(Constants.CRLF);
            stringBuilder.AppendFormat("Team2 Score:" + gameState.GetTeam2Score()).Append(Constants.CRLF);

            stringBuilder.Append("Ball:" + gameState.ball.GetComponent<Ball>().rb.position.x + " " + gameState.ball.GetComponent<Ball>().rb.position.y).Append(Constants.CRLF);
            stringBuilder.AppendFormat("PlayerList:").Append(Constants.CRLF);
            foreach (GameObject player in gameState.playerList)
            {
                stringBuilder.Append(Constants.CRLF).Append(buildPlayerBodyPart(player.GetComponent<Paddle>().GetID(), player.GetComponent<Paddle>().GetTeam(), player.GetComponent<Paddle>().rb.position.x, player.GetComponent<Paddle>().rb.position.y));
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Builds a Packet.
        /// </summary>
        public string buildPacket(string packetType)
        {
            GameManager gameState = (GameManager)GameObject.Find("GameManager").GetComponent("GameManager");
            string payload = "";
            StringBuilder stringBuilder = new StringBuilder(payload);
            //stringBuilder.Append(Constants.BOUNDARY).Append(Constants.CRLF);

            //// Header Info
            //stringBuilder.AppendFormat(Constants.CONTENTTYPEFORMAT, packetType).Append(Constants.CRLF).Append(Constants.CRLF);

            // TODO: Append Payload based on packetType
            switch (packetType)
            {
                case "Ball":
                    stringBuilder.Append(buildBallPositionBodyPart(gameState.ball.GetComponent<Ball>().rb.position.x, gameState.ball.GetComponent<Ball>().rb.position.y));
                    break;
                case "Player":
                    stringBuilder.Append(Constants.BOUNDARY).Append(Constants.CRLF);

                    // Header Info
                    stringBuilder.AppendFormat(Constants.CONTENTTYPEFORMAT, "Player").Append(Constants.CRLF).Append(Constants.CRLF);

                    stringBuilder.AppendFormat(buildPlayerBodyPart(gameState.localPlayer.GetComponent<Paddle>().GetID(), gameState.localPlayer.GetComponent<Paddle>().GetTeam(), gameState.localPlayer.GetComponent<Paddle>().rb.position.x, gameState.localPlayer.GetComponent<Paddle>().rb.position.y));
                    break;
                case "Player-Connection":
                    // TODO: Need to send payload containing their ID and current GameState - Done
                    stringBuilder.Append(buildPlayerConnectionPacket());
                    break;
                case "Player-Disconnect":
                    // TODO: Remove corresponding player from playerList in GameManager.
                    stringBuilder.Append(buildPlayerDisconnectBodyPart());
                    break;
                case "Update-GameState":
                    // TODO New player has joined Send UPDATE;
                    stringBuilder.Append(buildUpdateGameStatePacket());
                    break;

            }


            // Delimit end of message
            stringBuilder.Append(Constants.BOUNDARY + "--").Append(Constants.CRLF).Append(Constants.CRLF);
            stringBuilder.Append(Constants.EOT);
            return stringBuilder.ToString();

        }

        public int parseID(string payload)
        {
            int id = 0;
            Match m = Regex.Match(payload, Constants.IDREGEX);
            if (m.Success)
            {
                UnityEngine.Debug.Log("ID: " + m.Value.Split("id:")[1]);
                id = Int32.Parse(m.Value.Split("id:")[1]);
            }
            return id;
        }

        public float parseXCoord(string payload)
        {
            float XCoord = 0;
            Match m = Regex.Match(payload, Constants.XCOORDREGEX);
            if (m.Success)
            {
                UnityEngine.Debug.Log("XCOORD: " + m.Value.Split("Xcoord:")[1]);
                XCoord = float.Parse(m.Value.Split("Xcoord:")[1]);
            }
            return XCoord;
        }

        public float parseYCoord(string payload)
        {
            float YCoord = 0;
            Match m = Regex.Match(payload, Constants.YCOORDREGEX);
            if (m.Success)
            {
                UnityEngine.Debug.Log("YCOORD: " + m.Value.Split("Ycoord:")[1]);
                YCoord = float.Parse(m.Value.Split("Ycoord:")[1]);
            }
            return YCoord;
        }

        /// <summary>
        /// Sets the game state of the GameManager Singleton
        /// </summary>
        public void setGameState(int playerID, double xcoord, double ycoord)
        {
            GameManager gameState = GameManager.getInstance;
            //TODO https://stackoverflow.com/questions/53916533/setactive-can-only-be-called-from-the-main-thread
            foreach (GameObject player in gameState.playerList)
            {
                //if (playerID == player.GetComponent<Paddle>().id)
                //{
                //    // Update this Player.
                //}
            }
        }

        /// <summary>
        /// Parses incoming Packet.
        /// </summary>
        public string readPacket(string payload)
        {
            string id;
            float xCoord = -1;
            float yCoord = -1;
            GameManager gameState = GameManager.getInstance;
            //UnityEngine.Debug.Log(payload);
            // TODO: Detect Content-Type;
            Match m = Regex.Match(payload, Constants.CONTENTTYPEREGEX);
            if (m.Success)
            {
                //UnityEngine.Debug.Log("Recieved Content-Type: " + m.Value.Split("Content-Type:")[1]);
                string intermediate = m.Value.Split("Content-Type:")[1];
                switch (intermediate)
                {
                    case "Player-Connection":
                        processPlayerConnectionPacket(payload, gameState);                       
                        break;
                    case "Ball":                        
                        processBallPacket(payload, gameState);                        
                        break;                                        
                    case "Player":
                        processPlayerPacket(payload, gameState);                        
                        break;
                    case "Player-Disconnect":
                        processPlayerDisconnectPacket(payload, gameState);                     
                        break;
                    case "Update-GameState":
                        processUpdateGameStatePacket(payload, gameState); 
                       
                        //UnityEngine.Debug.Log(playerListInfo[1]);
                        break;
                }
            }

/*            id = parseID(payload);
            xCoord = parseXCoord(payload);
            yCoord = parseYCoord(payload);*/


            // After having parsed the incoming payload, set the state of the GameManager
            //setGameState(id, xCoord, yCoord);

            //return String.Format("Recieved Payload... [ID:{0} | XCOORD: {1} | YCOORD:{2}]", id.ToString(), xCoord.ToString(), yCoord.ToString());

            return "x:" + xCoord + '\n' + "y:" + yCoord;

        }

        void processPlayerConnectionPacket(string payload, GameManager gameState)
        {           
            
            string[] playerConnectionInfo = payload.Split(Constants.CRLF);
         

            // Current set to == for testing purpose
            // This checks if incoming packet is from same if so it should reject but for testing its == 
            // Change to != when testing
            if (gameState.localPlayer.GetComponent<Paddle>().GetID() != playerConnectionInfo[3].Split(':')[1])
            {
                gameState.SendGameState();
            }

        }

        void processBallPacket(string payload, GameManager gameState)
        { 
            gameState.SetBall(float.Parse(payload.Split("Xcoord:")[1].Split('\n')[0]), float.Parse(payload.Split("Ycoord:")[1].Split('\n')[0]));
        }

        void processPlayerPacket(string payload, GameManager gameState)
        {
            //GameManager gameState = GameManager.getInstance;
            string[] playerMovementInfo = payload.Split(Constants.CRLF);          

            // Current set to == for testing purpose
            // This checks if incoming packet is from same if so it should reject but for testing its == 
            // Change to != when testing
            if (gameState.localPlayer.GetComponent<Paddle>().GetID() != playerMovementInfo[3].Split(':')[1])
            {
                gameState.UpdatePlayerPosition(playerMovementInfo[3].Split(':')[1], float.Parse(playerMovementInfo[5].Split(':')[1]), float.Parse(playerMovementInfo[6].Split(':')[1]));
            }
        }

        void processPlayerDisconnectPacket(string payload, GameManager gameState)
        {
            string id = payload.Split("id:")[1].Split("\n")[0];            
            gameState.DisconnectPlayer(id);
        }

        void processUpdateGameStatePacket(string payload, GameManager gameState)
        {
            string[] updateGameStateInfo = payload.Split(Constants.CRLF);

            gameState.SetTeam1Score(Int32.Parse(updateGameStateInfo[3].Split(':')[1]));
            gameState.SetTeam2Score(Int32.Parse(updateGameStateInfo[4].Split(':')[1]));
            string ballPosition = updateGameStateInfo[5].Split(':')[1];
            gameState.ball.GetComponent<Ball>().rb.position = new Vector2(float.Parse(ballPosition.Split(' ')[0]), float.Parse(ballPosition.Split(' ')[1]));

            string[] playerListInfo = payload.Split(Constants.BOUNDARY + "--")[0].Split("PlayerList:")[1].Split(Constants.CRLF + Constants.CRLF);

            foreach (string playerInfo in playerListInfo.Skip(1))
            {                
                string[] playerInfoLine = playerInfo.Split(Constants.CRLF);    
                string playerID = playerInfoLine[0].Split(':')[1];
                int team = Int32.Parse(playerInfoLine[1].Split(':')[1]);

                GameObject player = gameState.InstantiatePlayer(playerID, team);
                player.transform.position = new Vector2(float.Parse(playerInfoLine[2].Split(':')[1]), float.Parse(playerInfoLine[3].Split(':')[1]));
            }
        }
    }
}
