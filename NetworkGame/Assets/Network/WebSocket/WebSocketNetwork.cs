using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

using UnityEngine;
using static NetworkModule.PacketHandler;

//using GameThread = _4945_A2.Threads.GameThread;
//using P = _4945_A2.Packet.Packet;

namespace NetworkModule
{
    public class WebSocketNetwork : Network
    {
        private HubConnection connection;

        //private static WebSocketNetwork Instance = new WebSocketNetwork();

        private WebSocketNetwork() { }

        private Queue<PacketHandler.Packet> packetQueue = new Queue<PacketHandler.Packet>();
        private object queueLock = new System.Object();
        private static PacketHandler packet = new PacketHandler();

        //public static WebSocketNetwork GetWebSocketNetwork() { return Instance; }
        //public WebSocketNetwork(GameThread gt) : base(gt)
        //{
        //}

        //public WebSocketNetwork(int port, string ipAddress, GameThread gt) : base(port, ipAddress, gt)
        //{
        //}

        //public WebSocketNetwork(int port, string ipAddress, GameThread gt, int bufferSize) : base(port, ipAddress, gt, bufferSize)
        //{
        //}

        public override void Send(string packet)
        {
            Console.WriteLine("SEND: " + packet.ToString());
            byte[] b = ASCIIEncoding.ASCII.GetBytes(packet);
            connection.SendAsync("SendPacket", b);
        }

        public override void Setup()
        {
            connection = new HubConnectionBuilder().WithUrl("http://192.168.0.242:5000/gamehub").Build();
            Console.WriteLine("Connection " + connection.ToString());
            connection.StartAsync().Wait();
            Console.WriteLine(connection.State + " " + connection.ConnectionId);
        }

        protected override void Receive()
        {
            connection.On("RecievePacket", (byte[] packet) =>
            {
               enqueuePacket(packet);
            });
        }

        void enqueuePacket(byte[] data)
        {
            lock (queueLock)
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
                packetQueue = new Queue<PacketHandler.Packet>();
            }
            foreach (PacketHandler.Packet packetTemp in temp)
            {
                packet.readPacket(Encoding.ASCII.GetString(packetTemp.data));
                //Debug.Log("MAIN THREAD READ98098"+Encoding.ASCII.GetString(packetTemp.data));
            }
        }

    }
}

    