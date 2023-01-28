using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;

namespace NetworkModule
{

    public class Multicast : MonoBehaviour
    {
        private static IPAddress mcastAddress;
        private static int mcastPort;
        private static Socket mcastSocket;
        private static MulticastOption mcastOption;
        private static IPAddress localIP;
        private static EndPoint localEP;
        private static IPEndPoint groupEP;
        private static EndPoint remoteEP;
        private static PacketHandler packet = new PacketHandler();
        private static string ip;
        private Queue<PacketHandler.Packet> packetQueue = new Queue<PacketHandler.Packet>();
        private object queueLock = new System.Object();

        /// <summary>
        /// Initializes network with default IP and port number
        /// </summary>
        public void initDefaultNetwork()
        {
            // Create endpoint

            // Initialize the multicast address group and multicast port.
            // Both address and port are selected from the allowed sets as
            // defined in the related RFC documents. These are the same 
            // as the values used by the sender.

            // Multicast Address that the reciever will 'subscribe' to
            mcastAddress = IPAddress.Parse("230.0.0.10");

            // Multicast Port
            mcastPort = 11000;

            try
            {
                // Set local IPaddress to Any
                //localIP = IPAddress.Any;
                ip = LocalIPAddress();
                localIP = IPAddress.Parse(ip); // CHANGE TO LOCAL IP ON LAN ROUTER
                Debug.Log(localIP.ToString());
                // IPAddress.Any works sometimes
                // Test:
                // Both computers start with IPAddress.Any DOES NOT SEND/REC PACKETS
                // Both computers switch to static IP DOES SEND/REC PACKETS
                // One computer switches back to IPAddress.Any Other continues to run from previous test DOES SEND/REC PACKETS
                // Other computer switches back to IPAddress.Any DOES SEND/REC PACKETS
                // Both computers stop, wait 5-15 sec start at same time DOES SEND/REC PACKETS
                // Both computers stop, wait 1-2 min start at same time DOES NOT SEND/REC PACKETS ??????????

                // Create new Socket, defining the AddressFamily, SocketType, and ProtocolType
                mcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                //mcastSocket.EnableBroadcast = true;
                // Endpoint to Bind to
                localEP = (EndPoint)new IPEndPoint(localIP, mcastPort);
                // Endpoint to send to
                groupEP = new IPEndPoint(mcastAddress, mcastPort);

                // Set socket option to reuse address
                mcastSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);

                // Bind to IPEndpoint
                mcastSocket.Bind(localEP);

                Thread receivingThread = new Thread(new ThreadStart(Receive));
                receivingThread.IsBackground = true;
                receivingThread.Start();

                // Create Thread to listen for incomming messages
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        /// <summary>
        /// Sends Yeet over socket connection
        /// </summary>
        /// <returns>
        /// <c>True</c> if messgae successfully sends.
        /// <c>False</c> otherwise
        /// </returns>
        public bool Send(string payload)
        {
            //if (socket == null) {
            //    System.Diagnostics.Debug.WriteLine("Network not initialized");
            //    return false;
            //}
            //string msg = "Yeet";
            //System.Diagnostics.Debug.WriteLine("Message sent: " + msg);
            //return true;

            try
            {

                //mcastSocket.SendTo(ASCIIEncoding.ASCII.GetBytes("Hello Multicast Listener"), groupEP);
                // Testing PacketBuilder
                //PacketHandler packet = new PacketHandler(); DEPRECATED
                GameManager currentGameState = GameManager.getInstance;

                // Send Packet that only contains localPlayer's position
                mcastSocket.SendTo(ASCIIEncoding.ASCII.GetBytes(payload), groupEP);
                //Debug.Log("Multicast data sent.....");
                return true;
            }
            catch (Exception e)
            {
                Debug.Log("\n" + e.ToString());
                return false;
            }

        }

  
        /// <summary>
        /// Recieves Payload from Socket Connection Subscribed to Sender. 
        /// Delegate method: To be run in a background thread, to 'receive' constantly.
        /// </summary>
        public void Receive()
        {
            // Create Multicast Option to set later
            mcastOption = new MulticastOption(mcastAddress, localIP);

            // Set socket options. Subscribe (Add Membership) to Multicast Broadcast
            mcastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcastOption);

            byte[] message = new byte[2048];

            // Endpoint to recieve message from (Any Ip Address, Port: 0)
            remoteEP = new IPEndPoint(IPAddress.Any, 0);

            //Debug.Log("Waiting for packets..");
            // Recieved bytes
            int recv = mcastSocket.ReceiveFrom(message, ref remoteEP);
            

            //string packetReceived = packet.readPacket(Encoding.ASCII.GetString(message));
            
            while (recv != 0)
            { 
                enqueuePacket(message);                
                message= new byte[2048];
             /*   Debug.Log("Recieved packets.. HERER\n" + packet.readPacket(Encoding.ASCII.GetString(message)));
                Debug.Log("Recieved Packets.. \n" + Encoding.ASCII.GetString(message));*/
                recv = mcastSocket.ReceiveFrom(message, ref remoteEP);
            }
 
        }

        void enqueuePacket(byte[] data)
        {
            lock(queueLock)
            {
                packetQueue.Enqueue(new PacketHandler.Packet(data));
            }
        }

        private void Update()
        {
            processPackets();
        }

        void processPackets()
        {
            Queue<PacketHandler.Packet> temp;

            lock (queueLock)
            {
                temp = packetQueue;
                //Debug.Log("Inside PROCESSES PACKET QUQUQUQUEUEE"+ temp.Count);
                packetQueue= new Queue<PacketHandler.Packet>();
            }
            foreach (PacketHandler.Packet packetTemp in temp)
            {
                packet.readPacket(Encoding.ASCII.GetString(packetTemp.data));
                //Debug.Log("MAIN THREAD READ98098"+Encoding.ASCII.GetString(packetTemp.data));
            }
        }

        public string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "0.0.0.0";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }

        public string GetIP()
        {
            return ip;
        }
        

    }
}
