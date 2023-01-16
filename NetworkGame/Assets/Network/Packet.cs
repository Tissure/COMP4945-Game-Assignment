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
    public class Packet
    {
        public static class Constants
        {
            public const string BOUNDARY = "boundary";
            public const string CRLF = "\r\n";
            public const string CONTENTTYPEFORMAT = "Content-Type:{0}";
            public const string IDFORMAT = "id:{0}";
            public const string COORDFORMAT = "{0}coord:{1}";
            public const string EOT = "\\4";

            public const string IDREGEX = @"\bid:\d*\b";
            public const string XCOORDREGEX = @"\bXcoord:\d*\b";
            public const string YCOORDREGEX = @"\bYcoord:\d*\b";
        }

        /// <summary>
        /// Builds a Packet.
        /// </summary>
        public string serverBuildPacket(int id, int xCoord, int yCoord)
        {
            string payload = "";
            StringBuilder stringBuilder = new StringBuilder(payload);

            stringBuilder.Append(Constants.BOUNDARY).Append(Constants.CRLF);

            stringBuilder.AppendFormat(Constants.CONTENTTYPEFORMAT, "Player").Append(Constants.CRLF).Append(Constants.CRLF);
            stringBuilder.AppendFormat(Constants.IDFORMAT, id).Append(Constants.CRLF);
            stringBuilder.AppendFormat(Constants.COORDFORMAT, "X", xCoord).Append(Constants.CRLF);
            stringBuilder.AppendFormat(Constants.COORDFORMAT, "Y", yCoord).Append(Constants.CRLF);

            // Delimit end of message
            stringBuilder.Append(Constants.BOUNDARY).Append(Constants.CRLF).Append(Constants.CRLF);
            stringBuilder.Append(Constants.EOT);
            return stringBuilder.ToString();

        }

        /*    public void buildPacket(Player player)
            {

            }*/

        /// <summary>
        /// Parses incoming Packet.
        /// </summary>
        public string readPacket(string payload)
        {
            int id = -1;
            int xCoord = -1;
            int yCoord = -1;

            Match m = Regex.Match(payload, Constants.IDREGEX);
            if (m.Success)
            {
                UnityEngine.Debug.Log("ID: " + m.Value.Split("id:")[1]);
                id = Int32.Parse(m.Value.Split("id:")[1]);
            }
            
            m = Regex.Match(payload, Constants.XCOORDREGEX);
            if (m.Success)
            {
                UnityEngine.Debug.Log("XCOORD: " + m.Value.Split("Xcoord:")[1]);
                xCoord = Int32.Parse(m.Value.Split("Xcoord:")[1]);
            } else
            {
                UnityEngine.Debug.Log("XCOORD: HELL");
            }

            m = Regex.Match(payload, Constants.YCOORDREGEX);
            if (m.Success)
            {
                UnityEngine.Debug.Log("YCOORD: " + m.Value.Split("Ycoord:")[1]);
                yCoord = Int32.Parse(m.Value.Split("Ycoord:")[1]);
            }
            else
            {
                UnityEngine.Debug.Log("YCOORD: HELL");
            }

            return String.Format("Recieved Payload... [ID:{0} | XCOORD: {1} | YCOORD:{2}]", id.ToString(), xCoord.ToString(), yCoord.ToString());

        }

    }
}
