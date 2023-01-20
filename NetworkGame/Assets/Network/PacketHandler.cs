using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace NetworkModule
{
    public class PacketHandler
    {
        public static class Constants
        {
            public const string BOUNDARY = "boundary";
            public const string CRLF = "\r\n";
            public const string CONTENTTYPEFORMAT = "Content-Type:{0}";
            public const string IDFORMAT = "id:{0}";
            public const string COORDFORMAT = "{0}coord:{1}";
            public const string EOT = "\\4";
            
            // Player Information
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
            stringBuilder.AppendFormat(Constants.IDFORMAT, gameState.localPlayer.id).Append(Constants.CRLF);
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
            GameManager gameState = (GameManager)GameObject.Find("GameManager").GetComponent("GameManager");
            string payload = "";
            StringBuilder stringBuilder = new StringBuilder(payload);
            stringBuilder.Append(Constants.BOUNDARY).Append(Constants.CRLF);

            // Header Info
            stringBuilder.AppendFormat(Constants.CONTENTTYPEFORMAT, "Player-Connection").Append(Constants.CRLF).Append(Constants.CRLF);

            // Payload - Current GameState + New Player's ID
            // New Player's ID
            stringBuilder.Append(buildPlayerConnectIDBodyPart());
            // Current List of Players
            stringBuilder.Append(buildPlayerListBodyPart(gameState.playerList));
            // Current Ball Position
            stringBuilder.Append(buildBallPositionBodyPart(gameState.ball.transform.position.x, gameState.ball.transform.position.y));

            // Delimit end of message
            //stringBuilder.Append(Constants.BOUNDARY).Append(Constants.CRLF).Append(Constants.CRLF);
            //stringBuilder.Append(Constants.EOT);

            return stringBuilder.ToString();

        }

        /// <summary>
        /// Helper Function to build Multi-Part bodypart
        /// </summary>
        /// <returns>MIME Multipart-Form Bodypart containing payload as a string.</returns>
        public string buildPlayerBodyPart(string id, float xCoord, float yCoord, string contentType)
        {
            string payload = "";
            StringBuilder stringBuilder = new StringBuilder(payload);

            stringBuilder.Append(Constants.BOUNDARY).Append(Constants.CRLF);
            // Header Info
            stringBuilder.AppendFormat(Constants.CONTENTTYPEFORMAT, contentType).Append(Constants.CRLF).Append(Constants.CRLF);

            // Payload
            stringBuilder.AppendFormat(Constants.IDFORMAT, id).Append(Constants.CRLF);
            stringBuilder.AppendFormat(Constants.COORDFORMAT, "X", xCoord.ToString()).Append(Constants.CRLF);
            stringBuilder.AppendFormat(Constants.COORDFORMAT, "Y", yCoord.ToString()).Append(Constants.CRLF);

            // Return payload
            return stringBuilder.ToString();

        }

        public string buildPlayerListBodyPart(List<Paddle> playerList)
        {
            GameManager gameState = (GameManager)GameObject.Find("GameManager").GetComponent("GameManager");
            string payload = "";
            StringBuilder stringBuilder = new StringBuilder(payload);

            stringBuilder.Append(Constants.BOUNDARY).Append(Constants.CRLF);

            // Header Info
            stringBuilder.AppendFormat(Constants.CONTENTTYPEFORMAT, "PlayerList").Append(Constants.CRLF).Append(Constants.CRLF);

            // Payload
            foreach(Paddle player in gameState.playerList)
            {
                stringBuilder.Append(buildPlayerBodyPart(player.id.ToString(), player.rb.position.x, player.rb.position.y, "playerList"));
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
                    break;
                case "Player":
                    stringBuilder.Append(buildPlayerBodyPart(gameState.localPlayer.id.ToString(), gameState.localPlayer.rb.position.x, gameState.localPlayer.rb.position.y, "Player"));
                    break;
                case "Player-Connection":
                    // TODO: Need to send payload containing their ID and current GameState - Done
                    stringBuilder.Append(buildPlayerConnectionPacket());
                    break;
                case "Player-Disconnect":
                    // TODO: Remove corresponding player from playerList in GameManager.
                    stringBuilder.Append(buildPlayerDisconnectBodyPart());
                    break;

            }


            // Delimit end of message
            stringBuilder.Append("--" + Constants.BOUNDARY + "--").Append(Constants.CRLF).Append(Constants.CRLF);
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

        public double parseXCoord(string payload)
        {
            double XCoord = 0;
            Match m = Regex.Match(payload, Constants.XCOORDREGEX);
            if (m.Success)
            {
                UnityEngine.Debug.Log("XCOORD: " + m.Value.Split("Xcoord:")[1]);
                XCoord = Double.Parse(m.Value.Split("Xcoord:")[1]);
            }
            return XCoord;
        }

        public double parseYCoord(string payload)
        {
            double YCoord = 0;
            Match m = Regex.Match(payload, Constants.YCOORDREGEX);
            if (m.Success)
            {
                UnityEngine.Debug.Log("YCOORD: " + m.Value.Split("Ycoord:")[1]);
                YCoord = Double.Parse(m.Value.Split("Ycoord:")[1]);
            }
            return YCoord;
        }

        /// <summary>
        /// Sets the game state of the GameManager Singleton
        /// </summary>
        public void setGameState(int playerID, double xcoord, double ycoord)
        {
            GameManager gameState = (GameManager)GameObject.Find("GameManager").GetComponent("GameManager");
            foreach (Paddle player in gameState.playerList)
            {
                if (playerID == player.id)
                {
                    // Update this Player.
                }
            }
        }

        /// <summary>
        /// Parses incoming Packet.
        /// </summary>
        public string readPacket(string payload)
        {
            int id = -1;
            double xCoord = -1;
            double yCoord = -1;

            // TODO: Detect Content-Type;
            Match m = Regex.Match(payload, Constants.CONTENTTYPEFORMAT);
            if (m.Success)
            {
                UnityEngine.Debug.Log("Recieved Content-Type: " + m.Value.Split("Content-Type:")[1]);
                string intermediate = m.Value.Split("Content-Type:")[1];
                switch (intermediate)
                {
                    case "Player-Connection":
                        // TODO: Parsing GameState: Part of "GameState" recieved when making connection to a Game.
                        break;
                    case "Ball":
                        // TODO: Parsing GameState: Part of "GameState" recieved when making connection to a Game.
                        break;
                    case "PlayerList":
                        // TODO: Parsing GameState: Part of "GameState" recieved when making connection to a Game.
                        break;
                    case "New-Player-ID":
                        // TODO: Recieving your new ID when connecting to a game.
                        break;
                    case "Player":
                        // Packet sent every frame from GameManager.Update()
                        break;
                    case "Player-Disconnect":
                        // TODO: Remove corresponding player from playerList in GameManager.
                        break;
                }
            }

            id = parseID(payload);
            xCoord = parseXCoord(payload);
            yCoord = parseYCoord(payload);


            // After having parsed the incoming payload, set the state of the GameManager
            setGameState(id, xCoord, yCoord);

            return String.Format("Recieved Payload... [ID:{0} | XCOORD: {1} | YCOORD:{2}]", id.ToString(), xCoord.ToString(), yCoord.ToString());

            //return "";

        }

    }
}
